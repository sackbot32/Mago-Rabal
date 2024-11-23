using System;
using UnityEngine;
public class BasicSpellTestA : SpellBase
{
    public void Hit(GameObject hitObj)
    {
        Debug.Log("Object hitted, name: " + hitObj);
    }

    public void Detonate()
    {
        Debug.Log("Detonation");
    }

    public void SelfCast(GameObject player)
    {
        Debug.Log("SelfCasted");
    }
}
