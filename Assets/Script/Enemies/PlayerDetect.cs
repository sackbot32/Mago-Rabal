using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    private IEnemyAI enemyAI;
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (transform.parent.gameObject.TryGetComponent(out IEnemyAI newEnemyAI))
        {
            enemyAI = newEnemyAI;
        } else
        {
            print("no ai");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Vector3 dir = other.transform.position - transform.parent.position;
            if (Physics.Raycast(transform.parent.position,dir.normalized,out RaycastHit hit, sphereCollider.radius))
            {
                if(hit.transform.gameObject == other.gameObject)
                {
                    enemyAI.SetPlayer(other.gameObject, true);
                } else
                {
                    enemyAI.SetPlayer(null, false);
                }
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyAI.SetPlayer(null, false);
        }
    }

}
