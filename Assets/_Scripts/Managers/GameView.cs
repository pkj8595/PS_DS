using UnityEngine;

public class GameView : MonoSingleton<GameView>
{
    [field: SerializeField] public GameObject _map { get; set; }
    [field: SerializeField] public GameObject _building { get; set; }
    [field: SerializeField] public GameObject _pawn { get; set; }

}
