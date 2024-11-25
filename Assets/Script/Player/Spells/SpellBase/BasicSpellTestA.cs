using System;
using System.Collections.Generic;
using UnityEngine;
public class BasicSpellTestA : SpellBase
{
    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        Debug.Log("A Object hitted, name: " + hitObj);
    }

    public void Detonate(List<SpellAtribute> atributes)
    {
        Debug.Log("A Detonation");
    }

    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        Debug.Log("A SelfCasted");
    }

    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {
        Debug.Log("A applied to " + proyectile.name);
    }
}
