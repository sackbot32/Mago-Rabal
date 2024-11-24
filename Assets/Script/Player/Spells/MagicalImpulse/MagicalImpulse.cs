using System.Collections.Generic;
using UnityEngine;

public class MagicalImpulse : SpellBase
{
    public float pushForceForPlayer = 2500;
    public float pushForceForEnem = 600;
    //Floor has more friction so needs extra force to do the same ammount of distance
    public float floorMultiplier = 2f;
    private string enemPushAtributeKey = "EnemPush";
    private string playerPushAtributeKey = "PlayerPush";
    private string playerPushMultAtributeKey = "PlayerPushMult";
    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        if(hitObj.GetComponent<MagicalImpulseEffect>() != null)
        {
            float trueEnemPushForce = pushForceForEnem;
            foreach (SpellAtribute atribute in atributes)
            {
                if(atribute.name == enemPushAtributeKey)
                {
                    trueEnemPushForce = atribute.value; 
                    break;
                }
            }
            hitObj.GetComponent<MagicalImpulseEffect>().force = trueEnemPushForce;
            hitObj.GetComponent<MagicalImpulseEffect>().enabled = true;
        } 
    }
    public void Detonate(List<SpellAtribute> atributes)
    {
        //Find a way to do this better
        MagicalImpulseEffect[] enemies = MagicalImpulseDetTarget.instance.ReturnList();
        foreach (MagicalImpulseEffect enemy in enemies)
        {
            if (enemy.enabled)
            {
                enemy.detonated = true;
            }
        }
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
}
