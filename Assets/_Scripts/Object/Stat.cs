using Cysharp.Threading.Tasks;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력바가 있는 모든 오브젝트에 사용
/// </summary>
public interface IStat
{
    public float Hp { get; set; }
    public float Mana { get; set; }
    public float MaxHp { get;}
    public float MaxMana { get;}

}

/// <summary>
/// 네트워크 동기화되는 기본 스탯 클래스 (PawnStat 병합)
/// </summary>
public class Stat : NetworkBehaviour, IStat
{
    // --- SyncVar<T> ---
    // 서버만 쓰기 권한, 클라이언트는 OnChange 콜백으로 변경 감지
    private readonly SyncVar<float> _syncedHp = new SyncVar<float>(new SyncTypeSettings(WritePermission.ServerOnly));
    private readonly SyncVar<bool> _syncedIsDead = new SyncVar<bool>(new SyncTypeSettings(WritePermission.ServerOnly));
    private readonly SyncVar<float> _syncedMana = new SyncVar<float>(new SyncTypeSettings(WritePermission.ServerOnly)); // Mana도 SyncVar로 관리

    // --- PawnStat에서 가져온 필드 ---
    private SOStatData _originData; // 원본 스탯 데이터 SO
    [field: SerializeField] public int KillCount { get; private set; } // TODO: 필요 시 SyncVar로 변경

    // --- 이벤트 ---
    // 스탯 변경 시 다른 컴포넌트에 알리기 위한 이벤트
    public event Action<float, float> OnHpChangedEvent; // currentHp, maxHp
    public event Action<bool> OnDeathStateChangedEvent; // isDead
    public event Action<float, float> OnManaChangedEvent; // currentMana, maxMana
    protected System.Action _OnDeadTargetEvent; // 기존 이벤트 유지 (서버에서 호출)
    protected System.Action _OnAffectEvent;     // 기존 이벤트 유지
    protected System.Action _OnChangeStatValueEvent; // 기존 이벤트 유지

    // --- IStat 인터페이스 구현 ---
    // SyncVar의 현재 값을 반환하도록 수정
    public float Hp { get => _syncedHp.Value; set => SetHp(value); } // Setter는 서버에서만 동작하도록 내부 처리
    public float Mana { get => _syncedMana.Value; set => SetMana(value); } // Setter는 서버에서만 동작하도록 내부 처리
    public bool IsDead { get => _syncedIsDead.Value; } // Getter만 제공


    public float BaseCoolDown => _originData?.baseSkillCooldown ?? 0f;
    public float SkillCoolDown => _originData?.skillCooldown ?? 0f;
    // --- PawnStat에서 가져온 프로퍼티 구현 ---
    public virtual float MaxHp => _originData?.hp ?? 1f; // 기본값 1로 설정 (null 방지)
    public virtual float MaxMana => _originData?.mana ?? 0f;
    public virtual float Defence => _originData?.defence ?? 0f;
    public virtual float SearchRange => _originData?.searchRange ?? 0f;
    public virtual float MoveSpeed => _originData != null ? (2f * (1f + _originData.movementSpeed)) : 0f;

    #region Network Lifecycle & Callbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
        // 서버 시작 시 스탯 초기화 (SO 데이터 기반)
        InitializeStats();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // 클라이언트 시작 시 SyncVar 변경 감지 콜백 등록
        _syncedHp.OnChange += OnHpChangedCallback;
        _syncedIsDead.OnChange += OnDeadStateChangedCallback;
        _syncedMana.OnChange += OnManaChangedCallback;

        // 클라이언트 로딩 시 초기값으로 콜백 강제 호출 (UI 등 초기 상태 반영)
        OnHpChangedCallback(_syncedHp.Value, _syncedHp.Value, false);
        OnDeadStateChangedCallback(_syncedIsDead.Value, _syncedIsDead.Value, false);
        OnManaChangedCallback(_syncedMana.Value, _syncedMana.Value, false);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        // 클라이언트 중지 시 콜백 해제
        if (_syncedHp != null) _syncedHp.OnChange -= OnHpChangedCallback;
        if (_syncedIsDead != null) _syncedIsDead.OnChange -= OnDeadStateChangedCallback;
        if (_syncedMana != null) _syncedMana.OnChange -= OnManaChangedCallback;
    }

    // HP SyncVar 변경 시 콜백
    private void OnHpChangedCallback(float prev, float next, bool asServer)
    {
        // 외부 구독자에게 이벤트 전달 (주로 클라이언트에서 UI 업데이트 등에 사용)
        OnHpChangedEvent?.Invoke(next, MaxHp);
    }

    // 사망 상태 SyncVar 변경 시 콜백
    private void OnDeadStateChangedCallback(bool prev, bool next, bool asServer)
    {
        // 외부 구독자에게 이벤트 전달 (주로 클라이언트에서 시각/물리 상태 변경에 사용)
        OnDeathStateChangedEvent?.Invoke(next);
    }

     // Mana SyncVar 변경 시 콜백
    private void OnManaChangedCallback(float prev, float next, bool asServer)
    {
        // 외부 구독자에게 이벤트 전달 (주로 클라이언트에서 UI 업데이트 등에 사용)
        OnManaChangedEvent?.Invoke(next, MaxMana);
    }
    #endregion

    #region Initialization
    // SO 데이터로 스탯 초기화 (Init 메서드 역할 통합)
    public virtual void InitializeFromSO(SOStatData statData)
    {
        _originData = statData;
        // 서버에서만 초기값 설정
        if (base.IsServerInitialized)
        {
            InitializeStats();
        }
    }

    // 서버에서 스탯 초기화 (체력, 마나, 사망 상태)
    private void InitializeStats()
    {
        if (!base.IsServerInitialized) return; // 서버 전용

        _syncedHp.Value = MaxHp;
        _syncedMana.Value = 0; // 초기 마나는 0 또는 MaxMana 등 게임 기획에 따라 설정
        _syncedIsDead.Value = false;
        KillCount = 0; // 킬 카운트 초기화
    }

    // 기존 Init 메서드는 콜백 등록용으로 남기거나 다른 방식으로 대체 가능
    // 여기서는 InitializeFromSO 를 사용하고, 콜백은 PawnBase 등에서 직접 등록하도록 변경
    /*
    public virtual void Init(SOStatData statData, System.Action onDead, System.Action onDeadTarget, System.Action onChangeStatValue)
    {
        _originData = statData; // SO 데이터 설정

        // 서버에서 초기 스탯 설정
        if (base.IsServerInitialized)
        {
             InitializeStats();
        }

        // 콜백 등록 (이제 사용하지 않거나 다른 방식으로 처리)
        // _OnDeadEvent -= onDead;
        // _OnDeadTargetEvent -= onDeadTarget;
        // _OnChangeStatValueEvent -= onChangeStatValue;
        // _OnDeadEvent += onDead;
        // _OnDeadTargetEvent += onDeadTarget;
        // _OnChangeStatValueEvent += onChangeStatValue;
    }
    */

     // 외부에서 콜백 등록/제거를 위한 메서드들 (필요 시)
    public void AddDeadTargetCallback(System.Action callback)
    {
        _OnDeadTargetEvent -= callback;
        _OnDeadTargetEvent += callback;
    }
     public void RemoveDeadTargetCallback(System.Action callback)
    {
        _OnDeadTargetEvent -= callback;
    }
    // ... 다른 콜백 등록/제거 메서드 ...

    #endregion

    #region Combat & Damage Handling
    /// <summary>
    /// 서버 권한: 데미지를 받아 체력을 변경하고 사망 여부를 처리합니다.
    /// </summary>
    /// <param name="msg">데미지 정보 메시지</param>
    [Server] // 이 메서드는 서버에서만 호출되어야 함을 명시 (FishNet 기능)
    public virtual void ReceiveDamage(ref DamageMessage msg)
    {
        if (_syncedIsDead.Value) return; // 이미 죽었으면 무시
        if (!base.IsServerInitialized) return; // 이중 체크 (Server 속성으로 충분할 수 있음)

        // 데미지 계산 (PawnStat에서 가져온 로직 사용)
        float finalDamage = CalculateDamage(msg, msg.attacker);

        // TODO: 버프/디버프 등 추가 효과 적용 (ApplyAffect 호출 등)
        // ApplyAffect(msg.skillAffectList, msg.attacker);

        if (finalDamage > 0)
        {
            _syncedHp.Value -= finalDamage; // SyncVar 값 변경 -> OnHpChangedCallback 호출됨
            // Debug.Log($"[Server] {gameObject.name} took {finalDamage} damage. HP: {_syncedHp.Value}/{MaxHp}");

            if (_syncedHp.Value <= 0)
            {
                _syncedHp.Value = 0; // HP가 0 미만으로 내려가지 않도록
                OnDead(msg.attacker); // 사망 처리 호출
            }
        }
    }

    // 데미지 계산 로직 (PawnStat에서 가져옴)
    public float CalculateDamage(DamageMessage msg, Stat attacker)
    {
        float rawDamage = 0f;
        // 예시: 스킬 데미지 또는 기본 공격 데미지 설정
        if (msg.skill != null && msg.skill.data != null)
        {
            // TODO: SO 스킬 데이터에서 실제 데미지 값 가져오기 (예: msg.skill.data.damageValue)
            // rawDamage = msg.skill.data.damageValue;
             rawDamage = 20f; // 임시 스킬 데미지
        }
        else
        {
            // TODO: 공격자의 기본 공격력 가져오기 (예: attacker.BaseAttackDamage)
            // rawDamage = attacker?.BaseAttackDamage ?? 5f;
             rawDamage = 10f; // 임시 기본 공격 데미지
        }

        // 방어력 적용 (기존 메서드 활용)
        float finalDamage = CalculateDamagePerProtection(rawDamage, this.Defence);

        // TODO: 속성 상성, 크리티컬, 회피 등 추가 계산

        return Mathf.Max(0, finalDamage); // 최종 데미지는 0 이상
    }

    // 방어력에 따른 데미지 감소 계산 (기존 메서드)
    public float CalculateDamagePerProtection(float damage, float protection)
    {
        if (protection >= 0)
        {
            // 방어력 공식: damage / (1 + protection * 0.01) - 필요시 수정
            return (damage / (1f + (protection * 0.01f)));
        }
        else
        {
            // 방어력이 음수일 경우 (방어력 감소 디버프 등) 데미지 증가 또는 그대로 반환
            // 예: return damage * (1 + Mathf.Abs(protection * 0.01f));
            return damage;
        }
    }

    // 서버 권한: 사망 처리
    [Server]
    protected virtual void OnDead(Stat attacker)
    {
        if (_syncedIsDead.Value) return; // 중복 사망 방지

        _syncedIsDead.Value = true; // SyncVar 변경 -> OnDeadStateChangedCallback 호출됨
        // Debug.Log($"[Server] {gameObject.name} has died.");

        // 공격자에게 타겟 사망 알림 (서버에서만 의미 있음)
        attacker?.OnDeadTarget();

        // 기존 OnDeadEvent 콜백은 제거됨 (OnDeathStateChangedEvent 사용)
        // _OnDeadEvent?.Invoke();
    }

    // 타겟 사망 시 호출 (서버에서 attacker가 호출)
    [Server]
    public virtual void OnDeadTarget()
    {
        // 이 메서드는 공격자 입장에서 타겟이 죽었을 때 호출됨
        KillCount++; // 킬 카운트 증가 (서버)
        _OnDeadTargetEvent?.Invoke(); // 구독된 콜백 실행
        // Debug.Log($"[Server] {gameObject.name} registered a kill. Total Kills: {KillCount}");
    }
    #endregion

    #region Stat Modification (Server-Side)
    // 서버에서 HP 설정 (외부 직접 호출 방지)
    [Server]
    private void SetHp(float value)
    {
        if (_syncedIsDead.Value) return; // 죽었으면 HP 변경 불가
        // 서버에서 값 변경 및 범위 제한
        _syncedHp.Value = Mathf.Clamp(value, 0, MaxHp);
    }

    // 서버에서 Mana 설정 (외부 직접 호출 방지)
    [Server]
    private void SetMana(float value)
    {
         _syncedMana.Value = Mathf.Clamp(value, 0, MaxMana);
    }

    // 서버에서 HP 회복
    [Server]
    public void RestoreHp(float amount)
    {
        if (_syncedIsDead.Value || amount <= 0) return;
        SetHp(_syncedHp.Value + amount);
         // Debug.Log($"[Server] {gameObject.name} restored {amount} HP. Current: {_syncedHp.Value}");
    }

     // 서버에서 Mana 사용/회복
    [Server]
    public bool TryConsumeMana(float amount)
    {
        if (amount <= 0) return true; // 0 또는 음수 소모는 항상 성공
        if (_syncedMana.Value >= amount)
        {
            SetMana(_syncedMana.Value - amount);
             // Debug.Log($"[Server] {gameObject.name} consumed {amount} Mana. Current: {_syncedMana.Value}");
            return true;
        }
        return false; // 마나 부족
    }
     [Server]
     public void RestoreMana(float amount)
    {
        if (amount <= 0) return;
        SetMana(_syncedMana.Value + amount);
         // Debug.Log($"[Server] {gameObject.name} restored {amount} Mana. Current: {_syncedMana.Value}");
    }


    // 서버 권한: 부활 처리
    [Server]
    public void Respawn()
    {
        if (!_syncedIsDead.Value) return; // 살아있으면 부활 불가
        // Debug.Log($"[Server] Respawning {gameObject.name}");
        InitializeStats(); // 스탯 초기화 (HP, Mana, IsDead 등)
    }

    #endregion

    #region Deprecated / Refactored Out
    // 기존 Hp, Mana 프로퍼티 직접 사용 및 OnAttacked 메서드는 제거됨
    // 기존 ApplyDamageMessage 는 ReceiveDamage 로 대체됨

    // 사용되지 않는 메서드들 주석 처리 또는 제거
    /*
    public virtual void OnLive()
    {
        // Respawn 메서드로 대체
    }

    public void SetActionOnChangeValue(System.Action onChangeStatValue) {}
    public void SetAffectEvent(System.Action affectAction) {}
    public void RemoveAffectEvent(System.Action affectAction) {}
    public void OnChangeStatValue() {}
    */

    #endregion

    #region Misc Getters (PawnStat에서 가져옴)
    public virtual float GetSkillCooldown()
    {
        return _originData?.skillCooldown ?? 0f;
    }

    public virtual float GetBaseSkillCooldown()
    {
        return _originData?.baseSkillCooldown ?? 0f;
    }
    #endregion
}
