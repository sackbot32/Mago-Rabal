using UnityEngine;

public class MagicalImpulse : SpellBase
{
    public float pushForce = 2500;
    public void Hit(GameObject hitObj)
    {
        if(hitObj.GetComponent<MagicalImpulseEffect>() != null)
        {
            hitObj.GetComponent<MagicalImpulseEffect>().enabled = true;
            Debug.Log("Enemy hit, impulse activated");
        } 
    }
    public void Detonate()
    {
        //Find a way to do this better
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Impulse detonated");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<MagicalImpulseEffect>().enabled)
            {
                enemy.GetComponent<MagicalImpulseEffect>().detonated = true;
            }
        }
    }


    public void SelfCast(GameObject player)
    {
        Debug.Log("Impulse self, source point: " + player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject);
        Vector3 dir = player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.forward;
        player.GetComponent<Rigidbody>().AddForce(dir * pushForce);
    }
}
