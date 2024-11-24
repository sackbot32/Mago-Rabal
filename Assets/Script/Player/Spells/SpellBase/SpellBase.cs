using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public interface SpellBase
{
    public void Hit(GameObject hitObj, List<SpellAtribute> atributes);

    public void Detonate(List<SpellAtribute> atributes);

    public void SelfCast(GameObject player, List<SpellAtribute> atributes);
    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes);
}
