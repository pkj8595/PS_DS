using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase : SOSkillModuleBase
{
    public abstract UniTask ApplyEffect(IAttackable caster, IDamageable target);
}

public abstract class SkillTargetingBase : SOSkillModuleBase
{
    public abstract List<IDamageable> FindTargets(IAttackable caster);
}

public abstract class SkillConditionBase : SOSkillModuleBase
{
    public abstract bool IsConditionMet(IAttackable caster);
    public abstract void ConsumeResources(IAttackable caster);
}

