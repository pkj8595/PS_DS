using UnityEngine;
using FishNet.Object.Synchronizing;
using static Define;

public class BuildingBase : Unit
{
    [SerializeField] private SOBuildingData buildingData;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRemderer;
    [field: SerializeField] public Vector3 StateBarOffset { get; set; }
    [field : SerializeField] public BuildingAniController AniController { get; private set; }

    protected BuildingDamageable _damageable { get; set; }
    public Stat Stat { get; private set; }
    public UnitSkill Skill { get; protected set; } = new();
    public Collider2D Collider { get => _collider; set => _collider = value; }
    public bool IsSelected { get; set; }

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

}
