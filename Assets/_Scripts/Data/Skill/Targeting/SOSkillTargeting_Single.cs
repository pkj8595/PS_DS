using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "SOSkillTargeting_Single", menuName = "Data/Skill/SkillTargeting/Single", order = 1)]
public class SOSkillTargeting_Single : SkillTargetingBase
{
    public float range = 1.0f;
    public Define.ETargetType targetType = Define.ETargetType.Enemy;

    public override List<IDamageable> FindTargets(IAttackable caster)
    {
        List<IDamageable> targets = new();
        if (Define.ETargetType.Self == targetType)
        {
            if (caster.Transform.TryGetComponent(out IDamageable damageable))
                targets.Add(damageable);
            return targets;
        }

        int layerTarget = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
        Collider2D [] colliders = Physics2D.OverlapCircleAll(caster.Transform.position, range, layerTarget);

        float minDistance = float.MaxValue;
        IDamageable target = null;
        foreach (var collider in colliders)
        {
            if (collider.transform == caster.Transform)
                continue;

            IDamageable unit = collider.attachedRigidbody.GetComponent<IDamageable>();
            if (unit != null && !unit.IsDead && caster.GetTargetType(unit.Team) == targetType)
            {
                float value = Vector3.Distance(caster.Transform.position, collider.transform.position);
                if (value < minDistance)
                {
                    minDistance = value;
                    target = unit;
                }
            }
        }
        if (target != null)
            targets.Add(target);

        return targets;
    }
}
