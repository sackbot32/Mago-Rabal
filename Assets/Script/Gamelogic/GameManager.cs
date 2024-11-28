using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public SpellSelector spellSelector;
    public DoorObject currentDoorObject;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    
    void Update()
    {
        
    }
}
