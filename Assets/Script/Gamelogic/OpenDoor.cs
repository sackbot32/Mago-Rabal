using DG.Tweening;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private Transform doorHinge;
    public InteractZone zone;

    public Vector3 targetRot;
    public float rotationDuration;

    private Vector3 originalRot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalRot = doorHinge.rotation.eulerAngles;
        zone.interactAction = Open;
        DOTween.Init();
    }

    public void Open()
    {
        if (Vector3.Angle(transform.right, transform.position - GameManager.instance.player.transform.position) <= 90f)
        {
            print("front");
            doorHinge.DORotate(originalRot + targetRot, rotationDuration);
        } else
        {
            print("back");
            doorHinge.DORotate(originalRot - targetRot, rotationDuration);
        }
        zone.interactAction = Close;
    }

    public void Close()
    {
        doorHinge.DORotate(originalRot, rotationDuration);
        zone.interactAction = Open;
    }
}
