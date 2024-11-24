using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellBase
{
    private float damage;
    private float explosiveDamage;

    private string damageKey = "Damage";
    private string expDamageKey = "ExplosionDamage";
    private string timeKey = "TimeBetween";

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
        float time = 0;
        float trueExplosionDamage = explosiveDamage;
        foreach (SpellAtribute atribute in atributes)
        {
            if (atribute.name == timeKey)
            {
                time = atribute.value;
            }
            if(atribute.name == expDamageKey)
            {
                trueExplosionDamage = atribute.value;
            }
        }
        FireballDetTarget.instance.ExplodeAllFireBalls(time,trueExplosionDamage);
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


    
}
