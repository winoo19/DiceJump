using UnityEngine;

public class HardcoreButton : MonoBehaviour
{
    public void SetGameState()
    {
        GameManager.gameState = GameManager.GameState.Hardcore;
    }
}
