using System.Collections.Generic;
using System;
using UnityEngine.AddressableAssets;


public class DataManager : ManagerBase
{
    public Dictionary<string, SOSkillData> SkillDatas { get; private set; } = new();
    public Dictionary<string, SOCharacterData> PawnDatas { get; private set; } = new();
    public Dictionary<string, SOBuildingData> BuildingDatas { get; private set; } = new();
    public Dictionary<string, SOGoodsData> GoodsDatas { get; private set; } = new();
    public Dictionary<string, SOStatData> StatDatas { get; private set; } = new();
    public Dictionary<string, SOProductionTable> ProductionTables { get; private set; } = new();



    public override void Init()
    {
        //Resources.LoadAll
        LoadTables();
    }

    private void LoadTables()
    {
        // "SkillData" 라벨이 붙은 모든 ScriptableObject 로드
        Addressables.LoadAssetsAsync<SOData>("Data", null).Completed += handle =>
        {
            foreach (var data in handle.Result)
            {
                switch(data)
                {
                    case SOSkillData skillData:
                        SkillDatas.Add(skillData.GetID(), skillData);
                        break;
                    case SOCharacterData pawnData:
                        PawnDatas.Add(pawnData.GetID(), pawnData);
                        break;
                    case SOBuildingData buildingData:
                        BuildingDatas.Add(buildingData.GetID(), buildingData);
                        break;
                    case SOGoodsData goodsData:
                        GoodsDatas.Add(goodsData.GetID(), goodsData);
                        break;
                    case SOStatData statData:
                        StatDatas.Add(statData.GetID(), statData);
                        break;
                    case SOProductionTable productionTable:
                        ProductionTables.Add(productionTable.GetID(), productionTable);
                        break;
                }
            }
            
            Debug.Log("Table Loaded!");
        };
    }

}
