using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FireballDetTarget : MonoBehaviour
{
    public static FireballDetTarget instance;
    public List<FireballDet> existingFireballs;

    private void Start()
    {
        instance = this;
        existingFireballs = new List<FireballDet>();
    }

    public void Add(FireballDet newFireballDet)
    {
        if (!existingFireballs.Contains(newFireballDet))
        {
            existingFireballs.Add(newFireballDet);
        }
    }

    public void Remove(FireballDet newFireballDet)
    {
        if (existingFireballs.Contains(newFireballDet))
        {
            existingFireballs.Remove(newFireballDet);
        }
    }

    public FireballDet[] ReturnList()
    {
        return existingFireballs.ToArray();
    }

    public void Clear()
    {
        foreach (var fireball in existingFireballs)
        {
            if (fireball != null)
            {
                Destroy(fireball.gameObject);
            }
        }
        existingFireballs.Clear();
    }

    public void ExplodeAllFireBalls(float timeBetweenExplosion,float explosionDamage)
    {
        StartCoroutine(ExplodeFireBallsWithTime(timeBetweenExplosion,explosionDamage));
    }

    private IEnumerator ExplodeFireBallsWithTime(float timeBetweenExplosion,float explosionDamage)
    {
        for (int i = 0; i < existingFireballs.Count; i++)
        {
            existingFireballs[i].explosionDamage = explosionDamage;
            existingFireballs[i].detonating = true;
            existingFireballs[i].Explode();
            yield return new WaitForSeconds(timeBetweenExplosion);
        }
        Clear();
    }
    
}
