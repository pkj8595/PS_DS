using UnityEngine;

[CreateAssetMenu(fileName = "SOStatData", menuName = "Data/SOStatData", order = 1)]
public class SOStatData : SOData
{
    public Define.EStatDataType statDataType;
    public float hp;
    public float mana;
    public float attack;
    public float defence;
    public float baseSkillCooldown;
    public float skillCooldown;
    public float movementSpeed;
    public float searchRange;
    //public float criticalHitChance;
    //public float criticalPer;
    //public float dodgepChance;
}
