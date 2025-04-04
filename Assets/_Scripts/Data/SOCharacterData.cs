using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "SOCharacterData", menuName = "Data/Character", order = 1)]
public class SOCharacterData : SOData
{
    public string title;
    public string desc;
    public int cost;
    public SOSkillData[] basicSkill;
    public SOStatData stat;
    public string Head;
    public string Ears;
    public string Eyes;
    public string Body;
    public string Hair;
    public string Armor;
    public string Helmet;
    public string Weapon;
    public string Shield;
    public string Cape;
    public string Back;
    public string Mask;
    public string Horns;
}


