using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : NetworkBehaviour
{
    public List<GameObject> AllUnits { get; private set; } = new List<GameObject>();
    public List<GameObject> AllBuildings { get; private set; } = new List<GameObject>();
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

    // 유닛 스폰 예시
    [ServerRpc]
    public void SpawnUnit(Vector3 position, GameObject unitPrefab)
    {
        GameObject unit = Instantiate(unitPrefab, position, Quaternion.identity);
        AllUnits.Add(unit);  // 리스트에 유닛 추가
        Spawn(unit);         // 네트워크 오브젝트로 동기화
    }

    // 건물 생성 예시
    [ServerRpc]
    public void SpawnBuilding(Vector3 position, GameObject buildingPrefab)
    {
        GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
        AllBuildings.Add(building);
        Spawn(building);
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
