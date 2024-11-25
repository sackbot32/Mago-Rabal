using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringGameObjectPair{

    public string name;
    public GameObject gameObject;

}

public class SpellDictionary : MonoBehaviour
{
    public static SpellDictionary instance;
    public Dictionary<SpellType, bool> spellDetonatedDict;
    public StringGameObjectPair[] forGameObjectDict;
    public Dictionary<string, GameObject> spellGameObjectDict;
    private void Awake()
    {
        instance = this;
        spellDetonatedDict = new Dictionary<SpellType, bool>();
        spellGameObjectDict = new Dictionary<string, GameObject>();
        foreach (SpellType spellType in Enum.GetValues(typeof(SpellType)))
        {
            spellDetonatedDict.TryAdd(spellType, false);
        }
        foreach (StringGameObjectPair pair in forGameObjectDict)
        {
            spellGameObjectDict.TryAdd(pair.name, pair.gameObject);
        }

    }
}

