using UnityEngine;

public class HighSpeedDamage : MonoBehaviour
{
    //Components
    private IHealth targetHealth;
    //Setting
    public float speedMagnitudeThreshold;
        //Should be taken by the magnitude of the speed
    public float damageMultiplier;

    private void Start()
    {
       gameObject.TryGetComponent(out targetHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > speedMagnitudeThreshold && collision.transform.tag != "Player" && collision.transform.tag != "Enemy")
        {
            print("Damage taken: " + collision.relativeVelocity.magnitude * damageMultiplier);
            targetHealth.TakeDamage(collision.relativeVelocity.magnitude * damageMultiplier);
        }
    }
}
