using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public InteractZone zone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zone.interactAction = Finish;
    }

    public void Finish()
    {
        GameManager.DestroySelfAndGoToScene(6);
    }
}
