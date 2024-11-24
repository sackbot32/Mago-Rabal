using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProyectile : MonoBehaviour
{
    //Components
    private GameObject hitParticle;
    //Settings
    public float timeTillDisable;
    public Action<GameObject, List<SpellAtribute>> onHitAction;
    public List<SpellAtribute> currentAtributes;
    public string[] tagsToCheck;
    [Tooltip("List of string of tags that it should just destroy agaisnt")]
    public string[] generalTagsToDeactivate;
    //Data
        //This will be used to choose the proyectile when using object pools
    public string proyectileName;

    private void Start()
    {
        StartCoroutine(DisableAfterTime());
    }

    //Will be extended as needed
    public void SetProyectileSettings(Action<GameObject, List<SpellAtribute>> newOnHit, List<SpellAtribute> newAtributes, string[] newTags,string newProyectileName ,
        GameObject newHitParticle)
    {
        proyectileName = newProyectileName;
        if(newHitParticle != null)
        {
            hitParticle = newHitParticle;
        }
        currentAtributes = newAtributes;
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
            onHitAction.Invoke(other.gameObject,currentAtributes);
            if(hitParticle != null)
            {
                Instantiate(hitParticle);
            }
            Destroy(gameObject);
        }

        if (hasDeactivationTag && !hasActionInvokeTag)
        {
            if (hitParticle != null)
            {
                Instantiate(hitParticle);
            }
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
