using System;
using UnityEngine;
[Serializable]
public class SpellAtribute
{
    public string name;
    public float value;

    public SpellAtribute(string newName, float newValue) 
    {
        name = newName;
        value = newValue;
    }

}
