
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Animations;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FireballEnemy : MonoBehaviour, IEnemyAI
{
    //Components
    private StateMachine brain;
    private NavMeshAgent agent;
    [SerializeField]
    private Transform aim;
    [SerializeField] 
    private ParentConstraint parentConstraint;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private ActionForAnimEvent animEvent;
    //Settings
    public Transform patrolPointsParent;
    public float tooCloseToPlayer;
    public float playerTooCloseToCover;
    public Transform shootPoint;
    public BaseSpellObject spellObject;
    public float feetDistanceDetect;
    //Data
    private List<Transform> coverPoints;
    private bool patrolling;
    private int currentPatrolPoint;
    public bool playerSeen;
    public GameObject player;
    private float distanceFromPlayer;
    private Vector3 lastPointSeen;
    private Coroutine shootCoroutine;
    private Vector3 previousPosition;
    private float curSpeed;
    private bool onGround;
    private LayerMask layer;
    void Start()
    {
        layer = LayerMask.GetMask("Ground");
        lastPointSeen = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponent<StateMachine>();
        //aim = transform.GetChild(0).transform;
        coverPoints = new List<Transform>();
        if(patrolPointsParent == null && GameObject.FindGameObjectWithTag("PatrolParent") != null)
        {
            patrolPointsParent = GameObject.FindGameObjectWithTag("PatrolParent").transform;
        }
        foreach (GameObject coverGM in GameObject.FindGameObjectsWithTag("Cover"))
        {
            coverPoints.Add(coverGM.transform);
        }
        if(animEvent != null)
        {
            animEvent.actionList.Add(ShootSpell);
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
        if (this.enabled)
        {
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;
            anim.SetFloat("Speed",curSpeed);
        }
        if (!onGround)
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, feetDistanceDetect, layer))
            {
                Debug.DrawRay(transform.position, -transform.up * feetDistanceDetect, Color.red);
                onGround = true;
                agent.enabled = true;
                if(player != null)
                {
                    ChooseCoverPoint();
                }
            }
        }
    }

    public void ImpulseEffect()
    {
        agent.enabled = false;
        onGround = false;
    }
    public void SetPlayer(GameObject newPlayer, bool detected)
    {
        playerSeen = detected;
        if(detected)
        {
            player = newPlayer;
        }
    }

    public void TurnOff()
    {
        agent.enabled = false;
        parentConstraint.enabled = false;
        this.enabled = false;
        anim.SetFloat("Speed", 0);
    }
    private void LookAtPlayer(bool doIt)
    {
        
        if (doIt && player != null)
        {
            aim.transform.forward = (player.transform.position - aim.transform.position).normalized;
            //aim.LookAt(player.transform.position);
            //aim.transform.localRotation = Quaternion.Euler(aim.rotation.eulerAngles.x, aim.rotation.eulerAngles.y, 0);
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
        if(shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
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
        if (agent.enabled)
        {
            agent.SetDestination(coverPoints[chosenCoverPoint].position);
        }
    }

    private IEnumerator ShootProcess()
    {
        while (true)
        {
            if(distanceFromPlayer > tooCloseToPlayer)
            {
                if(player != null && playerSeen)
                {
                    if (this.enabled)
                    {
                        anim.Play("lanzarhechizo",1);
                    } else
                    {
                        break;
                    }
                }
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
        Vector3 shootDir = player.transform.position - shootPoint.position;
        newProyectile.GetComponent<Rigidbody>().linearVelocity = shootDir.normalized * spellObject.proyectileSpeed;
        newProyectile.GetComponent<SpellProyectile>().SetProyectileSettings(SpellManager.ReturnSpell(spellObject.spellType).Hit, spellObject.atributes,
        spellObject.tagProyectileDetects, spellObject.spellProyectileName, spellObject.proyectileHitParticle);
    }
    #endregion
    #region CheckPlayerLastPos
    private void EnterCheck()
    {
        print("checking for player at " + lastPointSeen);
        LookAtPlayer(false);
        if (agent.enabled)
        {
            agent.SetDestination(lastPointSeen);
        }
    }

    private void ExecuteCheck()
    {

        if (playerSeen)
        {
            brain.PopState();
        } else
        {
            if(agent.enabled)
            {
                if(agent.remainingDistance < 1f)
                {
                    brain.PopState();
                    brain.PopState();
                }
            }
        }
    }
    private void ExitCheck()
    {
        
    }

    
    #endregion
}
