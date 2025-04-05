using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    private int _spawnCount;

    NetworkManager _networkManager;


    private void Awake()
    {
        _networkManager = GetComponentInParent<NetworkManager>();
        if (_networkManager == null)
            _networkManager = InstanceFinder.NetworkManager;

        if (_networkManager == null)
        {
            NetworkManagerExtensions.LogWarning($"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
            return;
        }

        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    private void OnDestroy()
    {
        _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    }

    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection connection, bool asServer)
    {
        if (!asServer)
            return;
        Debug.Log($"PlayerSpawn 실행됨! 현재 스폰 카운트: {_spawnCount}");

        if (_spawnCount >= _spawnPoints.Length)
        {
            Debug.LogWarning("더 이상 스폰할 수 없음!");
            return;
        }

        GameObject obj = Instantiate(_playerPrefab);
        obj.transform.SetPositionAndRotation(_spawnPoints[_spawnCount].position,
                                             _spawnPoints[_spawnCount].rotation);

        if (obj.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.Init(_spawnCount);
            Managers.Game.SetPlayer(playerController);
            _spawnCount++;
            Debug.Log($"오브젝트 {obj.name} 스폰 시작");
            _networkManager.ServerManager.Spawn(obj, connection);
        }
        else
        {
            Debug.Log($"스폰 실패 PlayerController is null");
        }

    }

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


    private void PlayerSpawn(NetworkConnection client = null)
    {
       
    }
}
