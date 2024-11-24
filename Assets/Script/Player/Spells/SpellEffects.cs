using System.Collections;
using UnityEngine;

public class SpellEffects : MonoBehaviour
{
    public void StartRecievedCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
