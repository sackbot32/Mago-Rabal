using System;
using System.Collections.Generic;
using UnityEngine;
public class BasicSpellTestA : SpellBase
{
    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        Debug.Log("Object hitted, name: " + hitObj);
    }

    public void Detonate(List<SpellAtribute> atributes)
    {
        Debug.Log("Detonation");
    }

    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        Debug.Log("SelfCasted");
    }

    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {
        Debug.Log("applied to " + proyectile.name);
    }
}
