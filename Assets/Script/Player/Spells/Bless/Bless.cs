using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Bless : SpellBase
{
    private string totalDamageKey = "TotalDamage";
    private string damageRateKey = "DamageRate";
    private string blessDurationKey = "BlessDuration";
    private string canBlowKey = "CanBlow";
    private string blessExplosionParticleKey = "BlessSpellExplosionParticle";
    private string blessEffectKey = "Blessed";
    private string blessExplosionEffectKey = "BlessedExplosion";
    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {

    }

    public void Detonate(List<SpellAtribute> atributes)
    {
        SpellDictionary.instance.spellDetonatedDict[SpellType.Bless] = true;
    }

    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        SpellDictionary.instance.spellDetonatedDict[SpellType.Bless] = false;
        float totalDamage = 5;
        float damageRate = 1;
        float timeToDealDamage = 5;
        bool canBlow = false;
        foreach (SpellAtribute atribute in atributes)
        {
            if (atribute.name == damageRateKey)
            {
                damageRate = atribute.value;
            }
            if (atribute.name == blessDurationKey)
            {
                timeToDealDamage = atribute.value;
            }
            if (atribute.name == canBlowKey)
            {
                canBlow = atribute.value == 1f;
            }
            if (atribute.name == totalDamageKey)
            {
                totalDamage = atribute.value;
            }
        }

        if (hitObj.tag == "Enemy")
        {
            hitObj.GetComponent<SpellEffects>().StartRecievedCoroutine(DamageOverTime(hitObj, totalDamage, damageRate, timeToDealDamage, canBlow), blessEffectKey);
        } else if (hitObj.tag == "Player")
        {
            hitObj.GetComponent<SpellEffects>().StartRecievedCoroutine(BlessedBuff(hitObj, totalDamage, damageRate, timeToDealDamage, canBlow), blessEffectKey);
        }
    }

    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        SpellDictionary.instance.spellDetonatedDict[SpellType.Bless] = false;
        float totalDamage = 5;
        float damageRate = 1;
        float timeToDealDamage = 5;
        bool canBlow = false;
        foreach (SpellAtribute atribute in atributes)
        {
            if (atribute.name == damageRateKey)
            {
                damageRate = atribute.value;
            }
            if (atribute.name == blessDurationKey)
            {
                timeToDealDamage = atribute.value;
            }
            if (atribute.name == canBlowKey)
            {
                canBlow = atribute.value == 1f;
            }
            if (atribute.name == totalDamageKey)
            {
                totalDamage = atribute.value;
            }
        }


        player.GetComponent<SpellEffects>().StartRecievedCoroutine(BlessedBuff(player, totalDamage, damageRate, timeToDealDamage, canBlow), blessEffectKey);
    }

    IEnumerator DamageOverTime(GameObject enemy, float totalDamage, float damageRate, float timeToDealDamage , bool canBlow)
    {
        float damagePerRate = (totalDamage * damageRate) / timeToDealDamage;
        float timer = 0f;
        IHealth enemyHealth = null;
        if( enemy.TryGetComponent(out IHealth health))
        {
            enemyHealth = health;
        }
       
        while (timer < timeToDealDamage)
        {
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerRate);
            }
            timer += damageRate;
            if (canBlow && SpellDictionary.instance.spellDetonatedDict[SpellType.Bless])
            {
                break;
            }
            yield return new WaitForSeconds(damageRate);
        }

        if (canBlow && SpellDictionary.instance.spellDetonatedDict[SpellType.Bless])
        {
            enemy.GetComponent<SpellEffects>().StartRecievedCoroutine(BlessExplosion(enemy, totalDamage, damageRate, timeToDealDamage), blessExplosionEffectKey);
        } else
        {
            enemy.GetComponent<SpellEffects>().corroutineNames.Remove(blessEffectKey);
        }

    }
    IEnumerator BlessedBuff(GameObject player, float totalDamage, float damageRate, float timeToDealDamage, bool canBlow)
    {
        float timer = 0f;
        player.GetComponent<PlayerHealth>().defenseOn = true;
        while (timer < timeToDealDamage)
        {
            timer += Time.deltaTime;
            if (canBlow && SpellDictionary.instance.spellDetonatedDict[SpellType.Bless])
            {
                break;
            }
            yield return null;
        }
        player.GetComponent<PlayerHealth>().defenseOn = false;
        if (canBlow && SpellDictionary.instance.spellDetonatedDict[SpellType.Bless])
        {
            player.GetComponent<SpellEffects>().StartRecievedCoroutine(BlessExplosion(player, totalDamage, damageRate, timeToDealDamage), blessExplosionEffectKey);
        } else
        {
            player.GetComponent<SpellEffects>().corroutineNames.Remove(blessEffectKey);
        }

    }

    private IEnumerator BlessExplosion(GameObject explosionSource,float totalDamage,float damageRate,float timeToDealDamage)
    {
        GameObject explosionPrefab;
        GameObject explosionParticlePrefab;
        SpellDictionary.instance.spellGameObjectDict.TryGetValue("SpellExplosion", out explosionPrefab);
        SpellDictionary.instance.spellGameObjectDict.TryGetValue(blessExplosionParticleKey, out explosionParticlePrefab);
        GameObject blessExplosion = GameObject.Instantiate(explosionPrefab, explosionSource.transform.position, Quaternion.identity);
        string[] list = new string[2];
        list[0] = "Enemy";
        list[1] = "Player";
        List<SpellAtribute> atributeForExplosion = new List<SpellAtribute>();
        atributeForExplosion.Add(new SpellAtribute(totalDamageKey, totalDamage));
        atributeForExplosion.Add(new SpellAtribute(damageRateKey, damageRate));
        atributeForExplosion.Add(new SpellAtribute(blessDurationKey, timeToDealDamage));
        atributeForExplosion.Add(new SpellAtribute(canBlowKey, 0));
        blessExplosion.GetComponent<SpellExplosion>().SetExplosionSettings(Hit, atributeForExplosion, list, explosionParticlePrefab);
        blessExplosion.GetComponent<SpellExplosion>().ExplodeParticle();
        //To give time for the object to not be affecte by its own explosion
        explosionSource.GetComponent<SpellEffects>().corroutineNames.Remove(blessExplosionEffectKey);
        yield return new WaitForSeconds(0.5f);
        explosionSource.GetComponent<SpellEffects>().corroutineNames.Remove(blessEffectKey);
    }
}
