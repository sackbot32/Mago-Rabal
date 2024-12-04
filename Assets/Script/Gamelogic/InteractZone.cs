using System;
using System.Collections;
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
        if(GameManager.instance.player != null)
        {
            player = GameManager.instance.player.transform;
            playerText = player.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>();
            playerText.enabled = false;
        } else
        {
            StartCoroutine(GetPlayer());
        }
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
        if(other.tag == "Player" && playerText != null)
        {
            if (!inside)
            {
                inside = true;
                if(key.Length  > 0)
                {
                    if (GameManager.instance.keys.Contains(key))
                    {
                        playerText.text = message;
                    } else
                    {
                        playerText.text = noKeyMessage;
                    }
                } else
                {
                    playerText.text = message;
                }
                playerText.enabled = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && playerText != null)
        {
            inside = false;
            playerText.enabled = false;
        }
    }

    void OnDestroy()
    {
        inside = false;
        if(playerText != null)
        {
            playerText.enabled = false;
        }
    }

    IEnumerator GetPlayer()
    {
        while(player == null)
        {
            if (GameManager.instance.player != null)
            { 
                player = GameManager.instance.player.transform;
            }
            yield return new WaitForSeconds(0.05f);
        }
        playerText = player.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>();
        playerText.enabled = false;
    }
}
