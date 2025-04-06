using FishNet.Object;
using FishNet.Object.Synchronizing;
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
    public Define.ETeam Team { get => (Define.ETeam)eTeam.Value; set => eTeam.Value = (int)value; }
    protected readonly SyncVar<int> eTeam = new SyncVar<int>();
    protected readonly SyncVar<string> DataId = new SyncVar<string>();

}