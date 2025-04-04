using UnityEngine;
using FishNet.Object;

public class TestPlayer : NetworkBehaviour
{

    void Awake()
    {

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("OnStartClient");
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TestServerRpc("ArrowTower1");
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TestServerRpc(string testStr)
    {
        Debug.Log($"TestServerRpc {testStr}");
    }


}
