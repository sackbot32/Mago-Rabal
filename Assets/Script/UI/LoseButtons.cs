using UnityEngine;

public class LoseButtons : MonoBehaviour
{
    public void GoToMenu()
    {
        GameManager.DestroySelfAndGoToScene(0);
    }

    public void RestartGame()
    {
        GameManager.DestroySelfAndGoToScene(1);
    }
}
