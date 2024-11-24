using UnityEngine;

public class MagicalImpulse : SpellBase
{
    public float pushForceForPlayer = 2500;
    public float pushForceForEnem = 600;
    //Floor has more friction so needs extra force to do the same ammount of distance
    public float floorMultiplier = 2f;
    public void Hit(GameObject hitObj)
    {
        if(hitObj.GetComponent<MagicalImpulseEffect>() != null)
        {
            hitObj.GetComponent<MagicalImpulseEffect>().force = pushForceForEnem;
            hitObj.GetComponent<MagicalImpulseEffect>().enabled = true;
            Debug.Log("Enemy hit, impulse activated");
        } 
    }
    public void Detonate()
    {
        //Find a way to do this better
        MagicalImpulseEffect[] enemies = MagicalImpulseDetTarget.instance.ReturnList();
        Debug.Log("Impulse detonated");
        foreach (MagicalImpulseEffect enemy in enemies)
        {
            if (enemy.enabled)
            {
                enemy.detonated = true;
            }
        }
    }


    public void SelfCast(GameObject player)
    {
        Debug.Log("Impulse self, source point: " + player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject);
        Vector3 dir = player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.forward;
        dir = new Vector3(dir.x, 0, dir.z);
        float truePushForce = player.GetComponent<PlayerControl>().DetectGround() ? pushForceForPlayer * floorMultiplier : pushForceForPlayer;

        player.GetComponent<Rigidbody>().AddForce(dir.normalized * truePushForce);
    }
}
