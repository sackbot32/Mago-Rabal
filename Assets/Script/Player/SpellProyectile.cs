using System;
using System.Collections;
using UnityEngine;

public class SpellProyectile : MonoBehaviour
{
    //Components
    //Settings
    public float timeTillDisable;
    public Action<GameObject> onHitAction;
    public string[] tagsToCheck;
    [Tooltip("List of string of tags that it should just destroy agaisnt")]
    public string[] generalTagsToDeactivate;

    private void Start()
    {
        StartCoroutine(DisableAfterTime());
    }

    //Will be extended as needed
    public void SetProyectileSettings(Action<GameObject> newOnHit, string[] newTags, Mesh newMesh)
    {
        if(GetComponentInChildren<MeshFilter>() != null && newMesh != null)
        {
            GetComponentInChildren<MeshFilter>().mesh = newMesh;
        }
        onHitAction = newOnHit;
        tagsToCheck = newTags;
    }


    private void OnTriggerEnter(Collider other)
    {
        bool hasActionInvokeTag = false;
        bool hasDeactivationTag = false;
        foreach (string tag in tagsToCheck)
        {
            if (other.tag == tag)
            {
                hasActionInvokeTag = true;
            }
        }
        foreach (string tag in generalTagsToDeactivate)
        {
            if(other.tag == tag)
            {
                hasDeactivationTag = true;
            }
        }

        if (hasActionInvokeTag)
        {
            onHitAction.Invoke(other.gameObject); 
            Destroy(gameObject);
        }

        if (hasDeactivationTag && !hasActionInvokeTag)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(timeTillDisable);
        //Change to disable later
        Destroy(gameObject);
    }
}
