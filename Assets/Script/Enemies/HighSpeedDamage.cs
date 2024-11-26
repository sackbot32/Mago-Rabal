using UnityEngine;

public class HighSpeedDamage : MonoBehaviour
{
    //Components
    private IHealth targetHealth;
    public GameObject testObject;
    //Setting
    public float speedMagnitudeThreshold;
        //Should be taken by the magnitude of the speed
    public float damageMultiplier;

    private void Start()
    {
       gameObject.TryGetComponent<IHealth>(out targetHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > speedMagnitudeThreshold)
        {
            print("Damage taken: " + collision.relativeVelocity.magnitude * damageMultiplier);
            targetHealth.TakeDamage(collision.relativeVelocity.magnitude * damageMultiplier);
        }
    }
}
