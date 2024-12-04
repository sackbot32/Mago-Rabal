using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IHealth
{
    //Components
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private Slider healthBar;
    //Settings
    public float maxHealth;

    //Data
    [SerializeField]
    private float currentHealth;
    private bool isDead;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();

        } else
        {
            anim.Play("recibirdano",2);
        }
        healthBar.value = currentHealth;
    }
    public void Heal(float healing)
    {
        currentHealth += healing;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.value = currentHealth;
    }
    public void Death()
    {
        healthBar.gameObject.SetActive(false);
        StartCoroutine(DeathProcces());
    }

    private IEnumerator DeathProcces()
    {
        
        if(gameObject.TryGetComponent(out IEnemyAI enemyAI))
        {
            enemyAI.TurnOff();
        }
        anim.Play("magomuerte");
        yield return new WaitForSeconds(2f);
        Instantiate(deathParticle,transform.position,Quaternion.identity);
        Destroy(gameObject);

    }
}
