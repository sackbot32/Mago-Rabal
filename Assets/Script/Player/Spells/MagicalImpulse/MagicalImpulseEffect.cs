using System.Collections;
using UnityEngine;

public class MagicalImpulseEffect : MonoBehaviour
{

    //Component
    private Rigidbody rb;
    private MagicalImpulseEffect selfImpulseEffect;
    //Setting
    public float timeTillAutoPush;
    public float force;
    //Data
    public bool detonated;
    private Coroutine coroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfImpulseEffect = this;
        selfImpulseEffect.enabled = false;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(PushCountDown());
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }




    private IEnumerator PushCountDown()
    {
        float timer = 0;
        while(!detonated && timer < timeTillAutoPush)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        //TODO Get where the player is looking
        rb.AddForce(transform.up * force);
        detonated = false;
        selfImpulseEffect.enabled = false;
    }
}
