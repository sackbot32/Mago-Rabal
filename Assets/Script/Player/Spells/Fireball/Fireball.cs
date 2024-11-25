using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellBase
{
    private float damage;
    private float explosiveDamage;
    public GameObject explosionPrefab;
    public GameObject explosionParticlePrefab;
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
        SpellDictionary.instance.spellDetonatedDict[SpellType.FireBall] = true;
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
        SpellDictionary.instance.spellDetonatedDict[SpellType.FireBall] = false;
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
        while (!SpellDictionary.instance.spellDetonatedDict[SpellType.FireBall])
        {
            yield return null;
        }
        SpellDictionary.instance.spellGameObjectDict.TryGetValue("SpellExplosion", out explosionPrefab);
        SpellDictionary.instance.spellGameObjectDict.TryGetValue("SpellExplosionParticle", out explosionParticlePrefab);
        if (explosionPrefab != null)
        {
            //Need to add proper explosion
            GameObject newExplo = GameObject.Instantiate(explosionPrefab,proyectile.transform.position,Quaternion.identity);
            string[] list = new string[1];
            list[0] = "Enemy";
            List<SpellAtribute> atributeForExplosion = new List<SpellAtribute>();
            atributeForExplosion.Add(new SpellAtribute(damageKey,explosionDamage));
            newExplo.GetComponent<SpellExplosion>().SetExplosionSettings(Hit,atributeForExplosion,list,explosionParticlePrefab);
            newExplo.GetComponent<SpellExplosion>().ExplodeParticle();

        }
        GameObject.Destroy(proyectile);
    }


}
