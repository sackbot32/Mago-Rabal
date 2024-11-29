using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public SpellSelector spellSelector;
    public DoorObject currentDoorObject;
    public List<string> keys;
    public List<string> enemies;
    void Awake()
    {
        if(instance == null)
        {
            keys = new List<string>();
            enemies = new List<string>();
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
