using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    //Components
    [SerializeField]
    private Animator anim;
    //Settings
    public float maxHealth;
    //Data
    [SerializeField]
    private float currentHealth;
    private bool isDead;
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

        } else
        {
            anim.Play("recibirdano",2);
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
        Destroy(gameObject);

    }
}
