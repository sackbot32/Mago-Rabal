using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    //Settings
    public float maxHealth;
    //Data
    [SerializeField]
    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();

        }
    }
    public void Heal(float healing)
    {
        currentHealth += healing;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
