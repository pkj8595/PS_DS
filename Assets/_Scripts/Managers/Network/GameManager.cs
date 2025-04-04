using FishNet.Object;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public List<PlayerController> _players = new();

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
        playerController.PlayerIndex++;
    }
}
