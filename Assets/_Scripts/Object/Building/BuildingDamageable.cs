using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDamageable : NetworkBehaviour, IDamageable
{
    private BuildingBase _buildingBase;
    [SerializeField] private Vector3 BuildingStateBarOffset;
    public Define.ETeam Team { get=> _buildingBase.Team;}
    public Vector3 StateBarOffset => _buildingBase.StateBarOffset;
    public Transform ProjectileTF => throw new System.NotImplementedException();
    public Stat Stat => _buildingBase.Stat;
    public ISkillMotion SkillMotion => _buildingBase.AniController;
    public Transform Transform => transform;
    public Collider2D Collider => _buildingBase.Collider;
    public bool IsDead => _buildingBase.Stat.IsDead;
    public virtual IStat GetIStat() => _buildingBase.Stat;

    public void Init(BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
    }


    public virtual bool ApplyTakeDamage(DamageMessage message)
    {
        _buildingBase.Stat.ApplyDamageMessage(ref message);

        return false;
    }


}
