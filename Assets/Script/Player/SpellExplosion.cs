using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class SpellExplosion : MonoBehaviour
{
    //Components
    private GameObject explodeParticle;
    //Settings
    public float timeTillDisable;
    public Action<GameObject, List<SpellAtribute>> onHitAction;
    public List<SpellAtribute> currentAtributes;
    public string[] tagsToCheck;

    private void Start()
    {
        StartCoroutine(DisableAfterTime());
    }
    public void ExplodeParticle()
    {
        if(explodeParticle != null)
        {
            Instantiate(explodeParticle,transform.position,Quaternion.identity);
        }
    }
    public void SetExplosionSettings(Action<GameObject, List<SpellAtribute>> newOnHit, List<SpellAtribute> newAtributes, string[] newTags,
        GameObject newParticle)
    {
        if(newParticle != null)
        {
            explodeParticle = newParticle;
        }
        currentAtributes = newAtributes;
        onHitAction = newOnHit;
        tagsToCheck = newTags;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hasActionInvokeTag = false;
        foreach (string tag in tagsToCheck)
        {
            if (other.tag == tag)
            {
                hasActionInvokeTag = true;
            }
        }

        if (hasActionInvokeTag)
        {
            onHitAction.Invoke(other.gameObject, currentAtributes);
        }

    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(timeTillDisable);
        //Change to disable later
        Destroy(gameObject);
    }
}
