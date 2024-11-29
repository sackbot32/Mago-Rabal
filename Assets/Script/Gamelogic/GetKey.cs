using UnityEngine;

public class GetKey : MonoBehaviour
{
    public InteractZone zone;
    public string keyName;
    void Start()
    {
        if(GameManager.instance != null)
        {
            if (GameManager.instance.keys.Contains(keyName))
            {
                Destroy(gameObject);
            } 
        }

        zone.interactAction = GiveKeyToPlayer;
    }

    public void GiveKeyToPlayer()
    {
        GameManager.instance.keys.Add(keyName);
        Destroy(gameObject);
    }    
}
