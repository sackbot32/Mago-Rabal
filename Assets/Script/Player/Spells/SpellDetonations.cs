using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellDetonations : MonoBehaviour
{
    public static SpellDetonations instance;
    public Dictionary<SpellType, bool> spellDetonatedDict;
    private void Awake()
    {
        instance = this;
        spellDetonatedDict = new Dictionary<SpellType, bool>();
        foreach (SpellType spellType in Enum.GetValues(typeof(SpellType)))
        {
            spellDetonatedDict.TryAdd(spellType, false);
        }
    }
}

