using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase : SOSkillModuleBase
{
    public abstract UniTask ApplyEffect(IDamageable caster, IDamageable target);
}

public abstract class SkillTargetingBase : SOSkillModuleBase
{
    public abstract List<IDamageable> FindTargets(IDamageable caster);
}

public abstract class SkillConditionBase : SOSkillModuleBase
{
    public abstract bool IsConditionMet(IDamageable caster);
    public abstract void ConsumeResources(IDamageable caster);
}

