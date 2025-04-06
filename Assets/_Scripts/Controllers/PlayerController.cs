using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Text;
using UnityEngine;
using static Define;

public class PlayerController : NetworkBehaviour
{
    public readonly SyncDictionary<EGoodsType, int> _dicGoods = new();

    public readonly SyncVar<int> PlayerIndex = new SyncVar<int>(-1);
    public readonly SyncVar<int> Life = new SyncVar<int>(100);
    public ETeam Team { get => (ETeam)PlayerIndex.Value; }


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (GetComponent<NetworkObject>() == null)
        {
            Debug.LogError("이 개체에는 NetworkObject가 없습니다!");
            return;
        }

        if (!IsSpawned)
        {
            Debug.LogError("네트워크 개체가 스폰되지 않았습니다!");
            return;
        }

        GetComponent<PlayerController>().enabled = IsOwner;
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //MapManager.Map.GetGridPos()
            Vector2Int cellPosition = Managers.Map.GetGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            string tableKey = "ArrowTower";
            Managers.Map.RpcBuildingRequest(cellPosition, tableKey, Team);
        }

    }

    public void Init(int playerIndex)
    {
        PlayerIndex.Value = playerIndex;
    }

    public void RequestBuild(int tableNum, Vector3 position)
    {

    }

    public void UpgradeBuilding(BuildingBase building)
    {

    }

    public void OnReceiveInput(/*InputData data*/)
    {

    }
 


}
