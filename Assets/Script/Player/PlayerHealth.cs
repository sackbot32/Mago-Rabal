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
    public float currentHealth;
    public bool defenseOn;
    void Start()
    {
        if(GameManager.instance.currentHealth <= 0)
        {
            currentHealth = maxHealth;
        } else
        {
            currentHealth = GameManager.instance.currentHealth;
        }
            healthBar.maxValue = maxHealth;
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
        GameManager.instance.currentHealth = currentHealth;
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
        GameManager.instance.currentHealth = currentHealth;
        if (currentHealth >= maxHealth )
        {
            currentHealth = maxHealth;
        }
    }

    public bool IsHealthFull()
    {
        return (currentHealth >= maxHealth);
    }
    public void Death()
    {
        GameManager.instance.LoseScreen();
    }
}
