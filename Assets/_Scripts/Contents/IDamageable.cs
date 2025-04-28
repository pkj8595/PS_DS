using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public interface IDamageable
{
    //프로퍼티
    public Define.ETeam Team { get;}
    public bool IsDead { get; }

    //필수 참조
    public Collider2D Collider { get; }
    public Transform Transform { get; }
    public Vector3 StateBarOffset { get; }

    //전투용 확장 인터페이스
    public ISkillMotion SkillMotion { get; }
    public Stat Stat { get; }

    //기능
    public bool ApplyTakeDamage(DamageMessage message);
}

public interface IAttackable
{
    public Define.ETeam Team { get; }
    public bool IsDead { get; }

    public Transform ProjectileTF { get; }
    public Transform Transform { get; }

    public ISkillMotion SkillMotion { get; }

}



// Unit 클래스 (V4 - SyncVar<T> 클래스 사용)
public class Unit : NetworkBehaviour
{
    // SyncVar<T> 클래스 인스턴스로 선언. readonly 권장.
    // 권한 설정은 SyncTypeSetting을 통해 전달.
    private readonly SyncVar<int> _team = new SyncVar<int>(0, 
        new SyncTypeSettings(WritePermission.ServerOnly, ReadPermission.Observers));

    private readonly SyncVar<string> _dataId = new SyncVar<string>("",
        new SyncTypeSettings(WritePermission.ServerOnly, ReadPermission.Observers));

    public Define.ETeam Team => (Define.ETeam)_team.Value; // 값 접근 시 .Value 사용
    public string DataId => _dataId.Value; // 값 접근 시 .Value 사용

    // OnStartServer, OnStartClient, LoadCharacterData 등은 이전과 유사하게 유지...
    // ...
    public override void OnStartServer()
    {
        base.OnStartServer();
        // 서버에서만 실행되는 초기화
        // 예: SetInitialData(determineTeam, determineDataId); // RPC 호출 대신 서버에서 직접 값 설정 가능
        // _team.Value = determineTeam;
        // _dataId.Value = determineDataId;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // 클라이언트에서만 실행되는 초기화
        // DataId 값 확인 및 로드
        if (!string.IsNullOrEmpty(_dataId.Value) && !base.IsServerInitialized)
        {
            LoadCharacterData(_dataId.Value);
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        // OnChange 콜백 등록 (Awake 또는 OnStartNetwork에서)
        _dataId.OnChange += OnDataIdChanged;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        // OnChange 콜백 해제 (OnStopNetwork 또는 OnDestroy에서)
        if (_dataId != null) _dataId.OnChange -= OnDataIdChanged;
    }

    
    // 서버에서 팀/데이터 ID 설정하는 메서드 예시 (값 설정 시 .Value 사용)
    [ServerRpc(RequireOwnership = false)]
    public void SetInitialData(int team, string dataId)
    {
        _team.Value = team;
        _dataId.Value = dataId;
    }

    // 클라이언트에서 DataId 변경 시 콜백 (시그니처 동일)
    private void OnDataIdChanged(string prev, string next, bool asServer)
    {
        if (!asServer)
        {
            LoadCharacterData(next);
        }
    }

    protected virtual void LoadCharacterData(string dataId)
    {
        Debug.Log($"Client loading character data for ID: {dataId}");
        // SO 로드 로직...
        // 예: CharacterData = Managers.Resource.Load<SOCharacterData>($"Data/Character/{dataId}");
        // 로드 후 PawnBase의 Init 같은 함수 호출 필요할 수 있음
    }
}
