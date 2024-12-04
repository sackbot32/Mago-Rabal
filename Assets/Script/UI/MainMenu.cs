using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Play()
    {
        GameManager.DestroySelfAndGoToScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
