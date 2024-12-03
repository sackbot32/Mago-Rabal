using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
    public InteractZone zone;
    public Transform appearPos;
    public DoorObject ownDoor;
    public DoorObject doorTarget;

    private void Start()
    {
        if(GameManager.instance.currentDoorObject != null)
        {
            if(ownDoor == GameManager.instance.currentDoorObject)
            {
                StartCoroutine(PutPlayerInPos());
            }
        }
        zone.interactAction = ChangeRoom;
    }
    public void ChangeRoom()
    {
        GameManager.instance.currentDoorObject = doorTarget;
        GameManager.instance.StartLoadingScene(doorTarget.sceneIndex);
    }

    private IEnumerator PutPlayerInPos()
    {
        while(GameManager.instance.player == null)
        {
            yield return new WaitForSeconds(0.01f);
        }
        GameManager.instance.FinishLoading();
        GameManager.instance.player.transform.position = appearPos.position;
        GameManager.instance.player.transform.rotation = appearPos.rotation;
    }
}
