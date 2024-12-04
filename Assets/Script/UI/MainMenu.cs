using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        GameManager.DestroySelfAndGoToScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
