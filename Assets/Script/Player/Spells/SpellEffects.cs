using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffects : MonoBehaviour
{
    public List<string> corroutineNames;

    private void Start()
    {
        corroutineNames = new List<string>();
    }
    public void StartRecievedCoroutine(IEnumerator coroutine,string name)
    {
        if (!corroutineNames.Contains(name))
        {
            corroutineNames.Add(name);
            StartCoroutine(coroutine);
        }
    }

}
