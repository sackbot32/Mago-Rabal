using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalImpulse : SpellBase
{

    public GameObject pushParticlePrefab;
    public float pushForceForPlayer = 2500;
    public float pushForceForEnem = 600;
    //Floor has more friction so needs extra force to do the same ammount of distance
    public float floorMultiplier = 2f;
    public float timeTillPush = 5f;
    private string enemPushAtributeKey = "EnemPush";
    private string playerPushAtributeKey = "PlayerPush";
    private string playerPushMultAtributeKey = "PlayerPushMult";
    private string timeTillPushKey = "TimeTillPush";
    private string impulseEffectKey = "Impulse";
    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        SpellDictionary.instance.spellDetonatedDict[SpellType.MagicalImpulse] = false;
        SpellEffects effectTarget = hitObj.GetComponent<SpellEffects>();
        if (effectTarget != null)
        {
            effectTarget.StartRecievedCoroutine(PushEnemiesUp(hitObj,atributes),impulseEffectKey);
        } 
    }

    IEnumerator PushEnemiesUp(GameObject hitObj, List<SpellAtribute> atributes)
    {
        float trueTimeTillPush = timeTillPush;
        float trueEnemyPush = pushForceForEnem;
        foreach (SpellAtribute atribute in atributes)
        {
            if(atribute.name == timeTillPushKey)
            {
                trueTimeTillPush = atribute.value; 
            }
            if(atribute.name ==  enemPushAtributeKey)
            {
                trueEnemyPush = atribute.value; 
            }
        }


        float timer = 0;
        while (!SpellDictionary.instance.spellDetonatedDict[SpellType.MagicalImpulse] && timer < trueTimeTillPush)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Detonado?: " + SpellDictionary.instance.spellDetonatedDict[SpellType.MagicalImpulse]);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(5);
        Vector3 impulseDir = targetPoint - hitObj.transform.position;
        SpellDictionary.instance.spellGameObjectDict.TryGetValue("PushParticle",out pushParticlePrefab);
        GameObject.Instantiate(pushParticlePrefab, hitObj.transform.position, Quaternion.identity).transform.up = impulseDir;
        hitObj.GetComponent<Rigidbody>()?.AddForce(impulseDir.normalized * trueEnemyPush);
        hitObj.GetComponent<SpellEffects>().corroutineNames.Remove(impulseEffectKey);

    }

    public void Detonate(List<SpellAtribute> atributes)
    {
        SpellDictionary.instance.spellDetonatedDict[SpellType.MagicalImpulse] = true;

    }


    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        float truePlayerPush = pushForceForPlayer;
        float truePlayerPushMult = floorMultiplier;
        foreach (SpellAtribute atribute in atributes)
        {
            if(atribute.name == playerPushAtributeKey)
            {
                truePlayerPush = atribute.value;
            }
            if(atribute.name == playerPushMultAtributeKey)
            {
                truePlayerPushMult = atribute.value;
            }
            if((truePlayerPushMult != floorMultiplier) && (truePlayerPush != pushForceForPlayer))
            {
                break;
            }
        }
        Vector3 dir = player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.forward;
        dir = new Vector3(dir.x, 0, dir.z);
        float truePushForce = player.GetComponent<PlayerControl>().DetectGround() ? truePlayerPush * truePlayerPushMult : truePlayerPush;

        player.GetComponent<Rigidbody>().AddForce(dir.normalized * truePushForce);
    }

    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {
        Debug.Log("Does nothing in this case");
    }
}
