using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private InputActionReference pauseInput;

    private bool isPaused;

    private void Start()
    {
        Time.timeScale = 1;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (pauseInput.action.WasPressedThisFrame())
        {
            isPaused = !isPaused;
            PauseGame(isPaused);
        }
    }

    public void PauseGame(bool pause)
    {
        if(pause && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if(!pause  && Time.timeScale != 1)
        {
            Time.timeScale = 1;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        isPaused = pause;
    }

    public void GoToMenu()
    {
        GameManager.DestroySelfAndGoToScene(0);
    }
}
