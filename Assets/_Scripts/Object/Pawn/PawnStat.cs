using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStat : Stat
{
    private SOStatData _originData;
    

    [field: SerializeField] public int KillCount { get; set; }
    public override float MaxHp { get => _originData.hp; }
    public override float MaxMana { get => _originData.mana; }
    public override float Defence { get => _originData.defence;}
    public float SearchRange => _originData.searchRange;
    public float MoveSpeed { get 
        {
            return 2f * (1f + (_originData.movementSpeed));
        }
    }


    public override void Init(SOStatData data, System.Action onDead, System.Action onDeadTarget, System.Action onChangeStatValue)
    {
        base.Init(data, onDead, onDeadTarget, onChangeStatValue);
        _originData = data;
        Hp = data.hp;
        Mana = 0;
    }

    public override void OnAttacked(float damageAmount, Stat attacker)
    {
        // 회피스탯 적용
        if (Random.Range(0, 1000) < 40)
        {
            string[] dodgeStr = {"느려", "운 좋게 피했다."};
            Managers.UI.ShowPawnDialog(transform, dodgeStr[Random.Range(0, dodgeStr.Length)]);
            return;
        }

        //todo effectManager damageNum
        float damage = Mathf.Max(0, CalculateDamagePerProtection(damageAmount, _originData.defence));
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    public override void OnDeadTarget()
    {
        base.OnDeadTarget();
        KillCount++;
    }


    public override float GetSkillCooldown()
    {
        return _originData.skillCooldown;
    }
    public override float GetBaseSkillCooldown()
    {
        return _originData.baseSkillCooldown;
    }

  
}
