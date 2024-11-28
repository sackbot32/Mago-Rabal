using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    //Components
    [SerializeField]
    private Slider healthBar;
    //Settings
    public float maxHealth;
    public float damageMultiplierForDefense;
    //Data
    [SerializeField]
    private float currentHealth;
    public bool defenseOn;
    void Start()
    {
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
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
        healthBar.value = currentHealth;
        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            Death();

        }
    }
    public void Heal(float healing)
    {
        currentHealth += healing;
        healthBar.value = currentHealth;
        if (currentHealth >= maxHealth )
        {
            currentHealth = maxHealth;
        }
    }
    public void Death()
    {
        print("Has muerto");
    }
}
