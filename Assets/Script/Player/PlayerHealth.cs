using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    //Settings
    public float maxHealth;
    public float damageMultiplierForDefense;
    //Data
    [SerializeField]
    private float currentHealth;
    public bool defenseOn;
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        if (defenseOn)
        {
            currentHealth -= damage*damageMultiplierForDefense;
        } else
        {
            currentHealth -= damage;
        }
        if(currentHealth <= 0 )
        {
            currentHealth = 0;
            Death();

        }
    }
    public void Heal(float healing)
    {
        currentHealth += healing;
        if(currentHealth >= maxHealth )
        {
            currentHealth = maxHealth;
        }
    }
    public void Death()
    {
        print("Has muerto");
    }
}
