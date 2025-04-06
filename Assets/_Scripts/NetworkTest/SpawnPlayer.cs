using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    private int _spawnCount;

    [SerializeField] private NetworkManager _networkManager;


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
        SetSpawn(obj.transform, out Vector3 position, out Quaternion rotation);

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
            _networkManager.ServerManager.Despawn(obj);
            Debug.Log($"스폰 실패 PlayerController is null");
        }

    }

    private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        //No spawns specified.
        if (_spawnPoints.Length == 0)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
            return;
        }

        Transform spawnPos = _spawnPoints[_spawnCount];
        if (spawnPos == null)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
        }
        else
        {
            pos = spawnPos.position;
            rot = spawnPos.rotation;
        }

        //Increase next spawn and reset if needed.
        _spawnCount++;
        if (_spawnCount >= _spawnPoints.Length)
            _spawnCount = 0;
    }

    private void SetSpawnUsingPrefab(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        pos = prefab.position;
        rot = prefab.rotation;
    }
}
