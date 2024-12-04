using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public float minHealing;
    public float maxHealing;

    private float healingAmount;
    void Start()
    {
        healingAmount = Random.Range(minHealing,maxHealing);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerHealth pHealth = other.GetComponent<PlayerHealth>();
            if (!pHealth.IsHealthFull())
            {
                pHealth.Heal(healingAmount);
                Destroy(gameObject);
            }
        }
    }
}
