using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public Stat attacker;
    public SkillEffectBase[] skillAffectList;
    public Skill skill;

    public DamageMessage(Stat attacker, Skill skill)
    {
        this.attacker = attacker;
        this.skill = skill;
        this.skillAffectList = skill.AffectList.ToArray();
    }
}
