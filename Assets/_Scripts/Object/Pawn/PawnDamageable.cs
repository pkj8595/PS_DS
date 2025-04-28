using FishNet.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PawnDamageable : NetworkBehaviour, IDamageable
{
    public PawnBase _pawnBase;
    [SerializeField] private Vector3 BuildingStateBarOffset;
    public Define.ETeam Team { get => _pawnBase.Team; }
    public Vector3 StateBarOffset => Vector3.up * 1.2f;
    [field: SerializeField] public Transform ProjectileTF { get; set; }
    public Stat Stat => _pawnBase.Stat;
    public ISkillMotion SkillMotion => _pawnBase.PawnAni;
    public Transform Transform => transform;
    public Collider2D Collider { get; private set; }
    public bool IsDead => _pawnBase.Stat.IsDead;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }

    public void Init(PawnBase pawnBase)
    {
        _pawnBase = pawnBase;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public bool ApplyTakeDamage(DamageMessage message)
    {
        _pawnBase.Stat.ReceiveDamage(ref message);
        return false;
    }

}
