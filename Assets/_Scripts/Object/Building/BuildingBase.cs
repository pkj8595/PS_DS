using UnityEngine;
using FishNet.Object.Synchronizing;
using static Define;

public class BuildingBase : Unit
{

    [SerializeField] private SOBuildingData buildingData;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRemderer;
    public Define.ETeam Team { get => (Define.ETeam)eTeam.Value; set => eTeam.Value = (int)value; }
    [field: SerializeField] public Vector3 StateBarOffset { get; set; }
    [field : SerializeField] public BuildingAniController AniController { get; private set; }

    protected BuildingDamageable _damageable { get; set; }
    public Stat Stat { get; private set; }
    public UnitSkill Skill { get; protected set; } = new();
    public Collider2D Collider { get => _collider; set => _collider = value; }
    public bool IsSelected { get; set; }


    private readonly SyncVar<string> DataId = new SyncVar<string>();
    private readonly SyncVar<int> eTeam = new SyncVar<int>();

    private void Awake()
    {
        if (Stat == null)
        {
            Stat = gameObject.GetComponent<PawnStat>();
        }
        if (_damageable == null)
        {
            _damageable = GetComponent<BuildingDamageable>();
        }
    }

    public void InitRpc(string buildingKey, Define.ETeam team)
    {
        if (Managers.Data.BuildingDatas.TryGetValue(buildingKey, out SOBuildingData soData))
        {
            DataId.Value = buildingKey;
            Init(soData, team);
        }
        else
            Debug.LogError($"Building data not found for tableKey: {buildingKey}");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (Managers.Data.BuildingDatas.TryGetValue(DataId.Value, out SOBuildingData soData))
        {
            Init(soData, Team);
        }
        else
            Debug.LogError($"Building data not found for tableKey: {DataId.Value}");
    }

    public virtual void Init(SOBuildingData data, Define.ETeam team)
    {
        buildingData = data;
        _spriteRemderer.sprite = data.tower;
        Team = team;

        Stat.Init(data.stat, OnDead, OnChagneStatValue, OnDeadTarget);
        Skill.Init(Stat.Mana);

        if (data.isDamageable)
        {
            if (_collider == null)
                _collider = GetComponent<Collider2D>();
            _damageable.enabled = true;
            _damageable.Init(this);

            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.AddUnit(_damageable);
        }
        else
        {
            _damageable.enabled = false;
            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.RemoveUnit(_damageable);
        }

    }


    private void DestroyComponent<T>() where T : MonoBehaviour
    {
        var skill = GetComponent<T>();
        if (skill != null)
        {
            Destroy(skill);
        }
    }

   /* public override bool UpgradeUnit()
    {
        *//*if (BuildingData.upgradeNum == 0 || Team == Define.ETeam.Enemy)
        {
            Managers.UI.ShowToastMessage("적은 업그레이드 할 수 없습니다.");
            return false;
        }

        if (Managers.Game.Inven.SpendItem(BuildingData.upgrade_goods, BuildingData.upgrade_goods_amount))
        {
            Init(BuildingData.upgradeNum);
            return true;
        }*//*

        Managers.UI.ShowToastMessage("업그레이드 비용이 부족합니다.");
        return false;
    }*/

    private void OnChagneStatValue()
    {

    }
   
    public void OnDeadTarget()
    {

    }

    private void OnDestroy()
    {
        if (_damageable != null)
        {
            UIStateBarGroup uiStatebarGroup = Managers.UI.GetUI<UIStateBarGroup>() as UIStateBarGroup;
            if (uiStatebarGroup != null)
                uiStatebarGroup.RemoveUnit(_damageable);
        }

    }

    public void OnDead()
    {
        if (_damageable != null)
        {
            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.SetActive(_damageable, false);
        }

        gameObject.SetActive(false);
    }

    public void OnSelect()
    {
        IsSelected = true;
    }

    public void OnDeSelect()
    {
        IsSelected = false;
    }

    
}
