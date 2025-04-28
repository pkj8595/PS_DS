using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 기본 스킬 클래스
public class Skill
{
    public SOSkillData data { get; private set; }
    public Sprite Icon { get => data.icon; }
    public string Name { get => data.name; }
    public string Desc { get => data.desc; }
    public float CoolTime { get => reducedCoolTime; }
    public float BaseCoolTime => data.coolTime;
    public int TableNum => data.tableNum;

    public List<SkillEffectBase> AffectList { get; } = new ();
    public float LastRunTime { get; set; }
    public bool IsBaseSkill { get; set; }
    public bool IsProcessing { get; private set; } = false;
    float reducedCoolTime = 0f;

    private Stat _stat;
    public Skill(SOSkillData skillData, Stat stat)
    {
        data = skillData;
        LastRunTime = -1000f;
        _stat = stat;
        //_stat.OnManaChangedEvent += CalculateCoolTime;
        CalculateCoolTime();
      
    }

    public void CalculateCoolTime()
    {
        if (IsBaseSkill)
            reducedCoolTime = BaseCoolTime / (1f + _stat.GetBaseSkillCooldown());
        else
            reducedCoolTime = BaseCoolTime / (1f + _stat.GetSkillCooldown());
    }

    public float GetCooldownProgress()
    {
        return 1f - Mathf.Clamp01(((Time.time - LastRunTime) / reducedCoolTime));
    }

    /// <summary>
    /// 스킬이 실행 가능한지 체크
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns>실행 가능하면 자원을 소모하고 true 반환</returns>
    public bool ReadySkill(IStat attacker)
    {
        if (IsReady(attacker.Mana))
        {
            LastRunTime = Time.time;
            attacker.Mana -= data.requireMana;
            return true;
        }
        return false;
    }

    public bool IsReady(float mana)
    {
        return !IsCooltime() && IsUseableMana(mana);
    }

    public bool IsCooltime()
    {
        bool isCooltime = Time.time < LastRunTime + reducedCoolTime;

        return isCooltime;
    }

    public bool IsUseableMana(float mana)
    {
        return mana >= data.requireMana; 
    }

    public float RemainCoolTime()
    {
        return CoolTime - (Time.time - LastRunTime);
    }

    public void ResetCoolTime()
    {
        LastRunTime = -1000f;
    }

    public bool StartSkill(IAttackable caster)
    {
        if (IsProcessing)
        {
            Debug.Log("스킬이 이미 실행 중입니다.");
            return false;
        }

        foreach (var condition in data.conditions)
        {
            if (!condition.IsConditionMet(caster))
            {
                Debug.Log("스킬 조건 미달");
                return false;
            }
        }

        data.conditions.ForEach(condition => condition.ConsumeResources(caster));

        StartSkillProcess(caster).Forget();

        return true;
    }

    public async UniTaskVoid StartSkillProcess(IAttackable caster)
    {
        IsProcessing = true;
        await Cast(caster);
        IsProcessing = false;
        LastRunTime = Time.time;
    }

    public async UniTask Cast(IAttackable caster)
    {
        SOSkillData skillData = data;

        if (skillData.castSkillMotion != ESkillMotionTriger.None)
            await caster.SkillMotion.RunAnimation(skillData.castSkillMotion, skillData.castDelayTime);

        // 애니메이션 시작 및 대기
        if (skillData.trigerSkillMotion != ESkillMotionTriger.None)
            await caster.SkillMotion.RunAnimation(skillData.trigerSkillMotion, skillData.trigerDelayTime);

        foreach (var skill in skillData.skills)
        {
            // 스킬 실행 시점에 타겟이 유효한지 다시 확인
            List<IDamageable> targets = skill.targeting.FindTargets(caster); // 타겟 스냅샷 저장

            foreach (IDamageable target in targets)
            {
                foreach (var effect in skill.effects)
                {
                    await effect.ApplyEffect(caster, target);
                }
            }
        }

    }
}
