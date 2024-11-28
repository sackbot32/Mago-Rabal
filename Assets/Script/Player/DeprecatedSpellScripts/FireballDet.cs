using UnityEngine;

public class FireballDet : MonoBehaviour
{
    public float explosionDamage;
    public bool detonating;
    private void OnEnable()
    {
        FireballDetTarget.instance.Add(this);
    }

    private void OnDisable()
    {
        if (!detonating)
        {
            FireballDetTarget.instance.Remove(this);
        }
    }

    public void Explode()
    {
        //Add explosion
        Debug.Log("Exploded with this damage: " + explosionDamage);
        gameObject.SetActive(false);
    }
}
