using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Transform player;
    public Animator animator;
    public LayerMask groundLayer, playerLayer;
    public float health;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange;
    public float attackRange;
    public int damage;
    public ParticleSystem hitEffect;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool takeDamage;

    public float range; //radius of sphere
    public Transform centrePoint;
    public float walkSpeed;
    public float attackSpeed;
    [HideInInspector] public bool playerInSightRange;
    [HideInInspector] public bool playerInAttackRange;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    navAgent.SetDestination(point);
                    navAgent.speed = walkSpeed;
                    animator.SetFloat("Speed", walkSpeed);
                }
            }
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            LookAtTarget();
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            LookAtTarget();
            AttackPlayer();
        }
        else if (!playerInSightRange && takeDamage)
        {
            LookAtTarget();
            ChasePlayer();
        }

        else if (!playerInSightRange && !takeDamage)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    navAgent.SetDestination(point);
                    navAgent.speed = walkSpeed;
                    animator.SetFloat("Speed", walkSpeed);
                }
            }
        }

    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void ChasePlayer()
    {
        navAgent.SetDestination(player.position);
        navAgent.speed = (attackSpeed * 10) - 2;
        animator.SetFloat("Speed", attackSpeed);
        navAgent.isStopped = false;
    }

    private void AttackPlayer()
    {
        navAgent.SetDestination(transform.position); // Stop moving

        if (!alreadyAttacked)
        {
            animator.SetBool("Shooting", playerInAttackRange);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Shooting", playerInAttackRange);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        hitEffect.Play();
        StartCoroutine(TakeDamageCoroutine());

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f);
        takeDamage = false;
    }

    private void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}