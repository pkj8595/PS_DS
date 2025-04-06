using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public enum EGameState
    {
        Lobby,
        Playing,
        Paused,
        Ended,
    }

    public List<PlayerController> _players = new();
    public readonly SyncVar<float> GameTimer = new();
    public readonly SyncVar<EGameState> GameState = new();

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

    internal void SetPlayer(PlayerController playerController)
    {
        _players.Add(playerController);
        playerController.PlayerIndex.Value++;
    }

    /// <summary>
    /// 게임 종료 시 모든 클라이언트에 알림.
    /// </summary>
    /// <param name="winnerPlayerId"></param>
    [ObserversRpc]
    public void NotifyGameEnd(int winnerPlayerId)
    {

    }
    [ObserversRpc]
    public void UpdateTimer(float time)
    {

    }
}
