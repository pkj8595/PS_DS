using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    private int _spawnCount;

    private void Awake()
    {
        InstanceFinder.NetworkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    private void OnDestroy()
    {
        InstanceFinder.NetworkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    }

    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection connection, bool arg2)
    {
        PlayerSpawn();
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


    [ServerRpc(RequireOwnership = false)]
    private void PlayerSpawn(NetworkConnection client = null)
    {
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
        }

        if (!obj.TryGetComponent<NetworkObject>(out var networkObject))
        {
            Debug.LogError("스폰할 오브젝트에 NetworkObject가 없음!");
            return;
        }

        Debug.Log($"오브젝트 {obj.name} 스폰 시작");
        Spawn(obj, client);
    }
}
