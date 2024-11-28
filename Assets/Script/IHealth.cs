using UnityEngine;

public interface IHealth
{
    public void TakeDamage(float damage);

    public void Heal(float healing);
    public void Death();
}
