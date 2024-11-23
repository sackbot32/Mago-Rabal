using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseSpellObject", menuName = "Scriptable Objects/BaseSpellObect")]
public class BaseSpellObject : ScriptableObject
{
    //Settings
    public Mesh spellProyectileMesh;
    public float proyectileSpeed;
    public float rate;
    public string[] tagProyectileDetects;
    public SpellType spellType;
    //Data
    public float timeSinceLastCast;
}
