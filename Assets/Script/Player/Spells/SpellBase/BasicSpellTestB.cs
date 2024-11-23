using System;
using UnityEngine;
public class BasicSpellTestB : SpellBase
{
    public void Hit(GameObject hitObj)
    {
        Debug.Log("B test " + hitObj);
    }

    public void Detonate()
    {
        Debug.Log("B test Detonation");
    }

    public void SelfCast(GameObject player)
    {
        Debug.Log("B test SelfCasted");
    }
}
