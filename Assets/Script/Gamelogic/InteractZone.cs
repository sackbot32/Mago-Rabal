using System;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InteractZone : MonoBehaviour
{
    //Components
    [SerializeField]
    private InputActionReference interactInput;
    //Setting
    public Action interactAction;
    public string key;
    public string message;
    public string noKeyMessage;
    public TMP_Text playerText;
    //Data
    public Transform player;
    private bool inside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.instance.player.transform;
        playerText = player.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>();
        playerText.enabled = false;
    }

    private void Update()
    {
        if(inside)
        {
            if(key.Length == 0)
            {
                if (interactInput.action.WasPressedThisFrame())
                {
                    if (interactAction != null)
                    {
                        interactAction.Invoke();
                    }
                    else
                    {
                        print("Interacted with no action");
                    }
                }
            } else
            {
                if (interactInput.action.WasPressedThisFrame() && GameManager.instance.keys.Contains(key))
                {
                    if (interactAction != null)
                    {
                        interactAction.Invoke();
                    }
                    else
                    {
                        print("Interacted with no action");
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!inside)
            {
                inside = true;
                if (GameManager.instance.keys.Contains(key))
                {
                    playerText.text = message;
                } else
                {
                    playerText.text = noKeyMessage;
                }
                playerText.enabled = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inside = false;
            playerText.enabled = false;
        }
    }
}
