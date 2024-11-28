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
        MagicalImpulseDetTarget.instance.Remove(selfImpulseEffect);
        selfImpulseEffect.enabled = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;

        }
        
    }

    private void OnEnable()
    {
        if(MagicalImpulseDetTarget.instance != null)
        {
            MagicalImpulseDetTarget.instance.Add(selfImpulseEffect);
        }
        coroutine = StartCoroutine(PushCountDown());
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        MagicalImpulseDetTarget.instance.Remove(selfImpulseEffect);
    }




    private IEnumerator PushCountDown()
    {
        float timer = 0;
        while(!detonated && timer < timeTillAutoPush)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(5);
        Vector3 impulseDir = targetPoint - transform.position;

        rb.AddForce(impulseDir.normalized * force);
        detonated = false;
        selfImpulseEffect.enabled = false;
    }
}
