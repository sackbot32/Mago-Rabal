using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellBase
{
    private float damage;
    private float explosiveDamage;
    public GameObject explosionPrefab;
    private string damageKey = "Damage";
    private string expDamageKey = "ExplosionDamage";

    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        float trueDamage = damage;
        foreach (SpellAtribute atribute in atributes)
        {
            if(atribute.name == damageKey)
            {
                trueDamage = atribute.value; 
                break;
            }
        }
        Debug.Log("Objective hit: " + hitObj.name + " damage dealth: " + trueDamage);

    }
    public void Detonate(List<SpellAtribute> atributes)
    {
        SpellDetonations.instance.spellDetonatedDict[SpellType.FireBall] = true;
    }

    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        float trueDamage = damage;
        foreach (SpellAtribute atribute in atributes)
        {
            if (atribute.name == damageKey)
            {
                trueDamage = atribute.value;
                break;
            }
        }
        Debug.Log("Objective hit: " + player.name + " damage dealth: " + trueDamage);
    }

    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {
        SpellDetonations.instance.spellDetonatedDict[SpellType.FireBall] = false;
        float trueExplosionDamage = explosiveDamage;
        foreach (SpellAtribute atribute in atributes)
        {

            if (atribute.name == expDamageKey)
            {
                trueExplosionDamage = atribute.value;
                break;
            }
        }
        SpellEffects effectTarget = proyectile.GetComponent<SpellEffects>();
        if (effectTarget != null)
        {
            effectTarget.StartRecievedCoroutine(ExplosionByDet(proyectile,trueExplosionDamage));
        }

    }

    IEnumerator ExplosionByDet(GameObject proyectile, float explosionDamage)
    {
        while (!SpellDetonations.instance.spellDetonatedDict[SpellType.FireBall])
        {
            yield return null;
        }
        if(explosionPrefab != null)
        {
            //Need to add proper explosion
            GameObject newExplo = GameObject.Instantiate(explosionPrefab,proyectile.transform.position,Quaternion.identity);

        }
        GameObject.Destroy(proyectile);
    }


}
