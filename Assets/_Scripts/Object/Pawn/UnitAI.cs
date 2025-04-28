using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitAI
{
    //option
    [field: SerializeField] public IDamageable LockTarget { get; set; }
    public bool HasTarget => LockTarget != null && !LockTarget.IsDead;
    public PawnBase Pawn { get; private set; }
    public float SearchRange;

    private float _cooltime = 0.0f;
    public void Init(PawnBase pawn)
    {
        Pawn = pawn;

    }

    public void OnUpdate()
    {
        _cooltime -= Time.deltaTime;
        if (_cooltime < 0)
        {
            _cooltime += 0.3f;
        }
        else
        {
        }
    }

    /// <summary>
    /// 타겟이 범위 밖으로 나갔다면 target추적 중지
    /// </summary>
    public bool CheckOutRangeTarget() 
    {
        if (SearchRange + 3 < Vector3.Distance(LockTarget.Transform.position, Pawn.transform.position) && Pawn.Team == Define.ETeam.Player1)
        {
            //SetState(GetReturnToBase());
            return true;
        }
        return false;
    }

    public bool CheckChangeState()
    {
        if (CheckOutRangeTarget())
            return true;

        return false;
    }

}
