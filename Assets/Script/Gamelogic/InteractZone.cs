using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InteractZone : MonoBehaviour
{
    //Components
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private InputActionReference interactInput;
    //Setting
    public Action interactAction;
    public string key;
    //Data

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.SetActive(false);
    }

    private void Update()
    {
        if(canvas.activeSelf)
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
            if (!canvas.activeSelf)
            {
                canvas.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(false);
        }
    }
}
