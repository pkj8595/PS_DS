using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : NetworkBehaviour
{
    public List<Unit> AllUnits { get; private set; } = new List<Unit>();
    public List<Unit> AllBuildings { get; private set; } = new List<Unit>();
    public Dictionary<string, int> PlayerResources { get; private set; } = new Dictionary<string, int>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log($"{this.name} 클라시작");
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log($"{this.name} 서버시작");
    }

   
    // 자원 획득 예시
    public void AddResource(string resourceType, int amount)
    {
        if (PlayerResources.ContainsKey(resourceType))
        {
            PlayerResources[resourceType] += amount;
            Debug.Log($"{resourceType} 증가: {PlayerResources[resourceType]}");
        }
    }

    // 자원 소비 예시
    public bool SpendResource(string resourceType, int amount)
    {
        if (PlayerResources.ContainsKey(resourceType) && PlayerResources[resourceType] >= amount)
        {
            PlayerResources[resourceType] -= amount;
            return true;
        }
        return false;  // 자원 부족 시 실패 처리
    }
}
