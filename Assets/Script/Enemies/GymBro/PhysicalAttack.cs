using System.Collections.Generic;
using UnityEngine;

public class PhysicalAttack : MonoBehaviour
{
    public Transform getForward;
    public List<string> tagToHit;
    public float damage;
    public float pushForce;
    public BoxCollider boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool contained = false;
        foreach (string item in tagToHit)
        {
            if (other.tag == item)
            {
                contained = true;
                break;
            }
        }

        if (contained)
        {
            if(other.TryGetComponent(out IHealth hitHealth))
            {
                DamageTarget(hitHealth,other.gameObject.GetComponent<Rigidbody>());
            }
        }
    }

    private void DamageTarget(IHealth health, Rigidbody rb = null) 
    {
        if(health != null)
        {
            health.TakeDamage(damage);
        }
        if(rb != null)
        {
            //Vector3 pushDir = (rb.transform.position - transform.position).normalized;
            rb.AddForce(getForward.forward*pushForce);
        }
    }
}
