using System;
using UnityEngine;
using UnityEngine.Events;

public interface SpellBase
{
    public void Hit(GameObject hitObj);

    public void Detonate();

    public void SelfCast(GameObject player);
}
