using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public Define.ETeam Team { get;}
    public Vector3 StateBarOffset { get; }
    public Transform ProjectileTF { get; }

    public abstract bool ApplyTakeDamage(DamageMessage message);
    public abstract Transform Transform { get; }
    public abstract Collider2D Collider { get; }

    public abstract bool IsDead { get; }

    public ISkillMotion SkillMotion { get; }
    public IStat GetIStat();
    public Stat Stat { get; }

}


public class Unit : NetworkBehaviour
{

}