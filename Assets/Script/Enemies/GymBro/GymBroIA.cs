using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Animations;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GymBroIA : MonoBehaviour, IEnemyAI
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
    [SerializeField]
    private PhysicalAttack attack;
    private AudioSource audioSource;
    //Settings
    public Transform patrolPointsParent;
    public float timeBetweenAttacks;
    public float tooCloseToPlayer;
    public float damage;
    public float pushForce;
    public List<string> tagsToHit;
    public float feetDistanceDetect;
    //Data
    private bool patrolling;
    private int currentPatrolPoint;
    public bool playerSeen;
    public GameObject player;
    private float distanceFromPlayer;
    private Vector3 lastPointSeen;
    private Coroutine attackCoroutine;
    private Vector3 previousPosition;
    private float curSpeed;
    private bool onGround;
    private LayerMask layer;
    void Start()
    {
        audioSource = attack.GetComponent<AudioSource>();
        layer = LayerMask.GetMask("Ground");
        lastPointSeen = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponent<StateMachine>();
        attack.damage = damage;
        attack.pushForce = pushForce;
        attack.tagToHit = tagsToHit;
        if (patrolPointsParent == null && GameObject.FindGameObjectWithTag("PatrolParent") != null)
        {
            patrolPointsParent = GameObject.FindGameObjectWithTag("PatrolParent").transform;
        }

        if (animEvent != null)
        {
            animEvent.actionList.Add(EnableAttackCollision);
            animEvent.actionList.Add(DisableAttackCollision);
        }
        brain.PushState(ExecutePatrol, EnterPatrol, ExitPatrol);
    }
    private void Update()
    {
        if (player != null && playerSeen)
        {
            distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        }
        else
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
            anim.SetFloat("Speed", curSpeed);
        }
        if (!onGround)
        {
            Debug.DrawRay(transform.position, -transform.up * feetDistanceDetect, Color.green);
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, feetDistanceDetect, layer))
            {
                onGround = true;
                agent.enabled = true;
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
        if (detected)
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
        }
        else
        {
            aim.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
    #region Patrol

    private void EnterPatrol()
    {
        patrolling = true;
        LookAtPlayer(false);
    }

    private void ExecutePatrol()
    {
        if (playerSeen)
        {
            brain.PushState(ExecuteAttack, EnterAttack, ExitAttack);
        }
    }

    private void ExitPatrol()
    {
        patrolling = false;
    }

    private void Patrol()
    {
        if (agent.destination == null || !agent.pathPending)
        {
            agent.SetDestination(patrolPointsParent.GetChild(currentPatrolPoint).position);
        }
        if (agent.remainingDistance < 1f)
        {
            currentPatrolPoint += 1;
            if (currentPatrolPoint >= patrolPointsParent.childCount)
            {
                currentPatrolPoint = 0;
            }
            agent.SetDestination(patrolPointsParent.GetChild(currentPatrolPoint).position);
        }
    }

    #endregion
    #region GoTottack
    private void EnterAttack()
    {
        //if in group, send to group
        if (player != null)
        {

        }

    }

    private void ExecuteAttack()
    {
        if (player != null)
        {
            lastPointSeen = player.transform.position;
            if (agent.enabled)
            {
                agent.SetDestination(player.transform.position);
            }
            LookAtPlayer(true);
        }
        //if player too close, move to another cover
        if (distanceFromPlayer <= tooCloseToPlayer)
        {
            if (attackCoroutine == null) 
            {
                attackCoroutine = StartCoroutine(AttackCourutine());
            }
        } else
        {
            if(attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
        if (!playerSeen)
        {

            brain.PushState(ExecuteCheck, EnterCheck, ExitCheck);
        }
    }

    private void ExitAttack()
    {
        LookAtPlayer(false);
    }

    IEnumerator AttackCourutine()
    {
        while (true)
        {
            anim.Play("pegargymbro", 1);
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    public void EnableAttackCollision()
    {
        audioSource.pitch = Random.Range(0.95f,1.05f);
        audioSource.Play();
        attack.boxCollider.enabled = true;
    }

    public void DisableAttackCollision()
    {
        audioSource.Stop();
        attack.boxCollider.enabled = false;
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
        }
        else
        {
            if (agent.enabled)
            {
                if (agent.remainingDistance < 1f)
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
