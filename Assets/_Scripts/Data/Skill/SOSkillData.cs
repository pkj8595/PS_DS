using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSkillData", menuName = "Data/Skill/SkillBase", order = 1)]
public class SOSkillData : SOData
{
    public string skillName;
    public string desc;
    public Sprite icon;
    public float coolTime;
    public float requireMana;

    [Space]
    public ESkillMotionTriger castSkillMotion;
    [ShowIfEnum("castSkillMotion", (int)ESkillMotionTriger.Cast)]
    public float castDelayTime;

    [Space]
    public ESkillMotionTriger trigerSkillMotion;
    [ShowIfEnum("trigerSkillMotion",(int)ESkillMotionTriger.Cast)]
    public float trigerDelayTime;

    public List<SkillConditionBase> conditions;
    public SkillGroup[] skills;

}


[System.Serializable]
public class SkillGroup
{
    public SkillTargetingBase targeting;
    public List<SkillEffectBase> effects;
}
