
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Animations;

public class FireballEnemy : MonoBehaviour, IEnemyAI
{
    //Components
    private StateMachine brain;
    private NavMeshAgent agent;
    private Transform aim;
    //Settings
    public Transform patrolPointsParent;
    public float tooCloseToPlayer;
    public float playerTooCloseToCover;
    public Transform shootPoint;
    public BaseSpellObject spellObject;
    //Data
    private List<Transform> coverPoints;
    private bool patrolling;
    private int currentPatrolPoint;
    public bool playerSeen;
    public GameObject player;
    private float distanceFromPlayer;
    private Vector3 lastPointSeen;
    private Coroutine shootCoroutine;
    void Start()
    {
        lastPointSeen = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponent<StateMachine>();
        aim = transform.GetChild(0).transform;
        coverPoints = new List<Transform>();
        if(patrolPointsParent == null && GameObject.FindGameObjectWithTag("PatrolParent") != null)
        {
            patrolPointsParent = GameObject.FindGameObjectWithTag("PatrolParent").transform;
        }
        foreach (GameObject coverGM in GameObject.FindGameObjectsWithTag("Cover"))
        {
            coverPoints.Add(coverGM.transform);
        }
        brain.PushState(ExecutePatrol,EnterPatrol,ExitPatrol);
    }
    private void Update()
    {
        if(player != null && playerSeen)
        {
            distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        } else
        {
            //LookAtPlayer(false);
        }
        if (patrolling && patrolPointsParent != null)
        {
            Patrol();
        }
    }

    public void SetPlayer(GameObject newPlayer, bool detected)
    {
        playerSeen = detected;
        if(detected)
        {
            player = newPlayer;
        }
    }

    private void LookAtPlayer(bool doIt)
    {
        
        if (doIt && player != null)
        {
            aim.transform.forward = (player.transform.position - aim.transform.position).normalized;
        } else
        { 
            aim.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
    #region Patrol

    private void EnterPatrol()
    {
        print(gameObject.name + " is patrolling");
        patrolling = true;
        LookAtPlayer(false);
    }

    private void ExecutePatrol()
    {
        if(playerSeen)
        {
            brain.PushState(ExectueCoverAttack,EnterCoverAttack,ExitCoverAttack);
        }
    }

    private void ExitPatrol()
    {
        patrolling = false;
    }

    private void Patrol()
    {
        if(agent.destination == null || !agent.pathPending)
        {
            agent.SetDestination(patrolPointsParent.GetChild(currentPatrolPoint).position);
        }
        if (agent.remainingDistance < 1f)
        {
            currentPatrolPoint += 1;
            if(currentPatrolPoint >= patrolPointsParent.childCount)
            {
                currentPatrolPoint = 0;
            }
            agent.SetDestination(patrolPointsParent.GetChild(currentPatrolPoint).position);
        }
    }

    #endregion
    #region GoToCoverAndAttack
    private void EnterCoverAttack()
    {
        //if in group, send to group
        ChooseCoverPoint();
        if(player != null)
        {
            shootCoroutine = StartCoroutine(ShootProcess());
        }
        
        print("going to cover at " + agent.destination);
    }

    private void ExectueCoverAttack()
    {
        if(player != null)
        {
            lastPointSeen = player.transform.position;
            LookAtPlayer(true);
        }
        //if player too close, move to another cover
        if(distanceFromPlayer < tooCloseToPlayer)
        {
            ChooseCoverPoint();
        } 
        if (!playerSeen)
        {

            brain.PushState(ExecuteCheck,EnterCheck,ExitCheck);
        }
    }

    private void ExitCoverAttack()
    {
        StopCoroutine(shootCoroutine);
        LookAtPlayer(false);
    }

    private void ChooseCoverPoint()
    {
        int chosenCoverPoint = 0;
        float closestDistance = float.MaxValue;
        float distance = 0;
        float distanceToPlayer = 0;
        foreach (Transform coverPoint in coverPoints)
        {
            distanceToPlayer = Vector3.Distance(coverPoint.position, player.transform.position);
            if (distanceToPlayer > playerTooCloseToCover)
            {
                distance = Vector3.Distance(coverPoint.position, transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    chosenCoverPoint = coverPoints.IndexOf(coverPoint);
                }
            }
            else
            {
                print("Too close to player in index" + coverPoints.IndexOf(coverPoint));
            }
        }
        agent.SetDestination(coverPoints[chosenCoverPoint].position);
    }

    private IEnumerator ShootProcess()
    {
        while (true)
        {
            if(distanceFromPlayer > tooCloseToPlayer)
            {
                ShootSpell();
                yield return new WaitForSeconds(spellObject.rate);
            } else
            {
                yield return null;
            }
        }
    }

    private void ShootSpell()
    {
        GameObject newProyectile = Instantiate(spellObject.spellProyectile, shootPoint.position, shootPoint.rotation);
        newProyectile.GetComponent<Rigidbody>().linearVelocity = shootPoint.forward * spellObject.proyectileSpeed;
        newProyectile.GetComponent<SpellProyectile>().SetProyectileSettings(SpellManager.ReturnSpell(spellObject.spellType).Hit, spellObject.atributes,
        spellObject.tagProyectileDetects, spellObject.spellProyectileName, spellObject.proyectileHitParticle);
    }
    #endregion
    #region CheckPlayerLastPos
    private void EnterCheck()
    {
        print("checking for player at " + lastPointSeen);
        LookAtPlayer(false);
        agent.SetDestination(lastPointSeen);
    }

    private void ExecuteCheck()
    {

        if (playerSeen)
        {
            brain.PopState();
        } else
        {
            if(agent.remainingDistance < 1f)
            {
                brain.PopState();
                brain.PopState();
            }
        }
    }
    private void ExitCheck()
    {
        
    }

    
    #endregion
}
