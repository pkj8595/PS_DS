using FishNet;
using UnityEngine;

public class ClientServerManager : MonoBehaviour
{
    [SerializeField] private bool isServer;


    private void Awake()
    {
        if (isServer)
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        
        }
        else
            InstanceFinder.ClientManager.StartConnection();
    }
}
