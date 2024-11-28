using System;
using System.Collections.Generic;
using UnityEngine;
public class BasicSpellTestB : SpellBase
{
    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        Debug.Log("B test " + hitObj);

    }

    public void Detonate(List<SpellAtribute> atributes)
    {
        Debug.Log("B test Detonation");
    }

    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        Debug.Log("B test SelfCasted");
    }

    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {
        Debug.Log("B test applied to " + proyectile.name);
    }
}
