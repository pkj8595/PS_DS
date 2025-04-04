using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MapManager : NetworkBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Dictionary<Vector2Int, MapObject> _dicMapObject = new();
    [SerializeField] private Dictionary<Vector2, BuildingBase> _dicBuilding = new();

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

    private void Update()
    {
        /*if (IsServerInitialized && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetGridPos(mousePos);
        }*/
    }

    public Vector2Int GetGridPos(Vector3 targetPos)
    {
        Vector3Int cellWorldToCellPos = _grid.WorldToCell(targetPos);
        Vector2Int cell2Pos = new Vector2Int(cellWorldToCellPos.x, cellWorldToCellPos.y);
        return cell2Pos;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RpcBuildingRequest(Vector2Int cellPos, string tableKey, Define.ETeam team, NetworkConnection client = null)
    {
        Debug.Log($"<color=green>RpcBuildingRequest:</color> {tableKey}");
        if (Managers.Data.BuildingDatas.TryGetValue(tableKey, out SOBuildingData soData))
        {
            Debug.Log($"<color=green>succese</color> {tableKey}");
            var obj = Managers.Resource.Instantiate("Network/BuildingBase");
            BuildingBase buildingBase = obj.GetComponent<BuildingBase>();
            buildingBase.transform.position = _grid.GetCellCenterWorld(new Vector3Int(cellPos.x, cellPos.y, 0));
            buildingBase.Init(soData, team);
            _dicBuilding.Add(cellPos, buildingBase);
            Spawn(obj, client);
            SetBuildingBase(obj, tableKey, team);
        }
        else
        {
            Debug.LogError($"Building data not found for tableKey: {tableKey}");
        }
    }

    [ObserversRpc]
    public void SetBuildingBase(GameObject obj, string tableKey, Define.ETeam team)
    {
        if (Managers.Data.BuildingDatas.TryGetValue(tableKey, out SOBuildingData soData))
        {
            BuildingBase buildingBase = obj.GetComponent<BuildingBase>();
            buildingBase.Init(soData, team);
        }
        else
        {
            Debug.LogError($"Building data not found for tableKey: {tableKey}");
        }
    }
}
