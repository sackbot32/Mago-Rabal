using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseSpellObject", menuName = "Scriptable Objects/BaseSpellObect")]
public class BaseSpellObject : ScriptableObject
{
    //Settings
    [Header("Funtional")]
    public GameObject spellProyectile;
    public Sprite spellSprite;
    public string spellProyectileName;
    public float proyectileSpeed;
    public float rate;
    public bool isAutomatic;
    public string[] tagProyectileDetects;
    public SpellType spellType;
    [SerializeField]
    public List<SpellAtribute> atributes;
    [Header("Visual")]
    public GameObject castParticle;
    public GameObject proyectileHitParticle;
    [Header("Audio")]
    public AudioClip castSound;
    public AudioClip detSound;
    //Data
    public float timeSinceLastCast;
}
