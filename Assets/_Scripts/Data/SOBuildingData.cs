using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBuildingData", menuName = "Data/Building", order = 1)]
public class SOBuildingData : SOData
{
    public Sprite tower;
    public string title;
    public string desc;
    public bool isDamageable;
    public SOStatData stat;
    public SOSkillData skill;

    public SOData[] slot; 
    public GoodsSlot upgradeGoods = new();
}

[System.Serializable]
public class GoodsSlot
{
    public SOGoodsData data;
    public int cost;
}