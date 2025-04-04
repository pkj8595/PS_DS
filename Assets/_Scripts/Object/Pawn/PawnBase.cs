using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public abstract class PawnBase :Unit, IDamageable
{
    //data
    public SOCharacterData CharacterData { get; private set; }
    public Define.ETeam Team { get; set; }

    //설정
    [field : SerializeField] protected Vector3 DestPos { get; set; }
    [field : SerializeField] public Vector3 OriginPosition { get; set; }
    [SerializeField] protected Define.EPawnAniState _state = Define.EPawnAniState.Idle;
    [SerializeField] protected Transform _projectileTrans;
    public Transform ProjectileTF => _projectileTrans;

    //pawn 기능
    [field : SerializeField] public PawnAnimationController AniController { get; private set; }
    [field : SerializeField] public PawnMove _pawnMove { get; set; }

    public PawnStat PawnStat { get; protected set; }
    public UnitSkill PawnSkills { get; } = new UnitSkill();
    public UnitAI AI { get; private set; }

    //option
    [field : SerializeField] public IDamageable LockTarget { get; set; }
    public bool HasTarget => LockTarget != null && !LockTarget.IsDead;
    public float SearchRange { get => PawnStat.SearchRange; }
    public float LastCombatTime { get; set; } = 0f;
    public Action OnDeadAction { get; set; }
    public Vector3 StateBarOffset => Vector3.up * 1.2f;
    private float speed = 2f;

    //component
    [SerializeField] private Collider2D _collider;
    [field: SerializeField] public string AIStateStr { get; set; }

    public bool IsSelected { get; set; }
    public virtual Define.EPawnAniState State
    {
        get { return _state; }
        set
        {
            _state = value;
            if (_state == Define.EPawnAniState.Ready || _state == Define.EPawnAniState.Idle)
            {
                if (Time.time < LastCombatTime + 5f)
                    _state = Define.EPawnAniState.Ready;
                else
                    _state = Define.EPawnAniState.Idle;
            }
            
            AniController.SetAniState(_state);
        }
    }

    public Stat Stat => PawnStat;

    public virtual void SetTriggerAni(Define.EPawnAniTriger trigger)
    {
        AniController.SetAniTrigger(trigger);
    }

    public virtual void Update()
    {
        AI?.OnUpdate();
    }

    private void OnDisable()
    {
        
    }

    public virtual void Init(SOCharacterData data, bool isUpgrade = false)
    {
        //component setting
        if (AniController == null)
            AniController = gameObject.GetComponentInChildren<PawnAnimationController>();

        if (PawnStat == null)
            PawnStat = gameObject.GetOrAddComponent<PawnStat>();


        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }

        if (_pawnMove == null)
        {
            _pawnMove = GetComponent<PawnMove>();
        }

        //table data setting
        CharacterData = data;
        AniController.Init(CharacterData);
        PawnStat.Init(CharacterData.stat, OnDead, OnDeadTarget, OnChangeStatValue);
        AI ??= new UnitAI();
        AI.Init(this);
        
        PawnSkills.Init(PawnStat.Mana);
        PawnSkills.SetBaseSkill(new Skill(CharacterData.basicSkill[0],PawnStat));

        //초기화 데이터 리셋
        if (!isUpgrade)
        {
        }
        
        //component data setting
        _collider.enabled = true;
        _pawnMove.Stop();
        _pawnMove.Init(PawnStat.MoveSpeed);

        //stateBar setting
        UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
        uiStatebarGroup.AddUnit(this);

    }

    public void UpdateMove()
    {
        if (_pawnMove.IsMove is false)
        {
            //AI.SetState(AI.GetIdleState());
        }

        AniController.Flip(_pawnMove.Velocity);

    }

    #region combat system
    /// <summary>
    /// 피격시 실행되는 함수 전달받은 매개변수로 캐릭터 데이터 갱신
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public  bool ApplyTakeDamage(DamageMessage message)
    {
        SetTriggerAni(Define.EPawnAniTriger.Hit);
        PawnStat.ApplyDamageMessage(ref message);
        LastCombatTime = Time.time;

        return false;
    }

    /// <summary>
    ///  Animation Event 함수 실행시
    /// </summary>
    public virtual void EndAniAttack()
    {
        PawnSkills.ClearCurrentSkill();
        LastCombatTime = Time.time;
        //AI.SetState(AI.GetIdleState());
    }
    #endregion

    /// <summary>
    /// pawn이 죽었을 때 실행되는 함수
    /// </summary>
    private void OnDead()
    {
        OnDeadAction?.Invoke();
        //AI.SetState(AI.GetDeadState());
        UIStateBarGroup uiStatebarGroup = Managers.UI.GetUI<UIStateBarGroup>() as UIStateBarGroup;
        uiStatebarGroup.SetActive(this, false);
        _collider.enabled = false;
        _pawnMove.Stop();

    }

    private void OnChangeStatValue()
    {
        speed = PawnStat.MoveSpeed;
    }



    private void OnLive()
    {
        if (IsDead)
        {
            PawnStat.OnLive();
            //AI.SetState(AI.GetIdleState());
            UIStateBarGroup uiStatebarGroup = Managers.UI.GetUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.SetActive(this, true);
            _collider.enabled = true;
            _pawnMove.Stop();
        }
    }

    private void OnDeadTarget()
    {
        
    }

    public void SetDestination(Vector3 position, bool isChase = true)
    {
        if (DestPos == position)
            return;
        
    }

    protected virtual void OnMove(Vector3 destPosition, bool isChase)
    {
        if (DestPos == destPosition)
            return;
        DestPos = destPosition;
        _pawnMove.Move(destPosition);
        
    }


    /// <summary>
    /// 이동 중지    /// </summary>
    public void OnMoveStop()
    {
        _pawnMove.Stop();
    }


    public Sprite GetIdleSprite()
    {
        return AniController.GetIdleSprite();
    }


    public Transform Transform => transform;
    public bool IsDead => PawnStat.IsDead;
    public IStat GetIStat() => PawnStat;
    public Collider2D Collider => _collider;
    public ISkillMotion SkillMotion => AniController;
}
