using System.Collections.Generic;
using UnityEngine;

// 상태 머신
public enum PawnStateType { Idle, Move, Attack }

public interface IPawnState
{
    void OnEnter(Unit unit);
    void Update();
    void OnExit();
}

public class PawnStateMachine
{
    private readonly Unit _owner;
    private readonly Dictionary<PawnStateType, IPawnState> _states = new();
    private IPawnState _current;

    public PawnStateMachine(Unit owner) => _owner = owner;

    public void Register(PawnStateType type, IPawnState state) => _states[type] = state;

    public void ChangeState(PawnStateType type)
    {
        _current?.OnExit();
        _current = _states[type];
        _current.OnEnter(_owner);
    }

    public void Update() => _current?.Update();
}

// 상태 구현
public class PawnIdleState : IPawnState
{
    private Unit _unit;
    public void OnEnter(Unit unit) => _unit = unit;

    public void Update()
    {
        /*var target = _unit.FindTargetInRange(_unit.Stat.AttackRange);
        if (target != null)
        {
            _unit.ChangeState(PawnStateType.Attack);
            return;
        }

        // 공격 대상 없으면 공격로 따라 이동
        _unit.ChangeState(PawnStateType.Move);*/
    }

    public void OnExit() { }
}

public class PawnMoveState : IPawnState
{
    private Unit _unit;
    public void OnEnter(Unit unit) => _unit = unit;

    public void Update()
    {
/*        var target = _unit.FindTargetInRange(_unit.Stat.AttackRange);
        if (target != null)
        {
            _unit.ChangeState(PawnStateType.Attack);
            return;
        }

        // 범위 밖이라면 그냥 끝까지 이동
        _unit.MoveTo(_unit.Stat.FinalMovePosition);*/
    }

    public void OnExit() { }
}

public class PawnAttackState : IPawnState
{
    private Unit _unit;
    private IDamageable _target;

    public void OnEnter(Unit unit)
    {
        /*_unit = unit;
        _target = _unit.FindTargetInRange(_unit.Stat.AttackRange);*/
    }

    public void Update()
    {
        /*if (_target == null || _target.IsDead || !_unit.IsInRange(_target, _unit.Stat.AttackRange))
        {
            _unit.ChangeState(PawnStateType.Move);
            return;
        }

        _unit.Attack(_target);*/
    }

    public void OnExit() { }
}
