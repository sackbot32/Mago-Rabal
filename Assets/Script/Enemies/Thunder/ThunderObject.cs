using System.Collections;
using UnityEngine;

public class ThunderObject : MonoBehaviour
{
    public Collider hitBox;
    public string hitTag;
    public float colliderWaitTillOn;
    public float colliderWaitTillOff;
    public float damage;
    private bool playerHit;
    private bool follow;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void SettingSetter(string newHitTag, float newColliderWaitTillOn, float newColliderWaitTillOff, float newDamage,Transform newPlayer)
    {
        hitTag = newHitTag;
        colliderWaitTillOn = newColliderWaitTillOn;
        colliderWaitTillOff = newColliderWaitTillOff;
        damage = newDamage;
        player = newPlayer;
        StartCoroutine(StartCollider());
    }
    void Start()
    {
        hitBox = GetComponent<Collider>();
        hitBox.enabled = false;
        
    }

    void Update()
    {
        if(player != null)
        {
            if(follow)
            {
                transform.position = player.transform.position;
            }
        }
    }

    IEnumerator StartCollider()
    {
        follow = true;
        yield return new WaitForSeconds(colliderWaitTillOn);
        follow = false;
        yield return new WaitForSeconds(colliderWaitTillOn/2f);
        hitBox.enabled = true;
        yield return new WaitForSeconds(colliderWaitTillOff);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == hitTag && !playerHit)
        {
            if (other.TryGetComponent(out IHealth health))
            {
                playerHit = true;
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
