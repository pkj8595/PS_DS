using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Text;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public readonly SyncDictionary<Define.EGoodsType, int> _dicGoods = new();

    public int PlayerIndex { get; internal set; }
    public Define.ETeam Team { get; internal set; }

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
            Debug.Log("call Managers.Map.GetGridPos");
            Vector2Int cellPosition = Managers.Map.GetGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            string tableKey = "ArrowTower";
            Debug.Log("Encoding: " + System.Text.Encoding.Default.EncodingName);
            Managers.Map.RpcBuildingRequest(cellPosition, tableKey, Team);
            SimpleTest();
        }

        if (!IsOwner || !IsSpawned || GetComponent<NetworkObject>() == null)
        {
            Debug.Log("Test if (!IsOwner || !IsSpawned || GetComponent<NetworkObject>() == null)");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Test(2);
        }
    }
    [ServerRpc]
    private void SimpleTest()
    {
        Debug.Log("✅ SimpleTest RPC called on server!");
    }

    [ServerRpc(RequireOwnership = false)]
    public void Test(int num)
    {
        Debug.Log("ServerRpc num : " + num);
        ObserverTest(num);
    }

    [ObserversRpc]
    public void ObserverTest(int num)
    {
        Debug.Log("ObserverTest num : " + num);
    }

    public void Init(int playerIndex)
    {
        PlayerIndex = playerIndex;
        Team = (Define.ETeam)playerIndex;
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
