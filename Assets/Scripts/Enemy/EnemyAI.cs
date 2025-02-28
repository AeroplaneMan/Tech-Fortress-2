using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

// This class controls the behavior of an enemy AI, including movement, attacking, and taking damage.
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

    public float range;
    public Transform centrePoint;
    public float walkSpeed;
    public float attackSpeed;
    [HideInInspector] public bool playerInSightRange;
    [HideInInspector] public bool playerInAttackRange;
    [SerializeField] private LevelCompleteCheck levelComplete;

    // Initializes references for NavMeshAgent, Animator, and Player.
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Updates the AI's behavior based on player proximity and various conditions.
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance) //Checks if enemy is done with path
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point)) //Pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //So you can see with gizmos
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
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    navAgent.SetDestination(point);
                    navAgent.speed = walkSpeed;
                    animator.SetFloat("Speed", walkSpeed);
                }
            }
        }
    }

    // Generates a random point within a specified range.
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

    // Rotates the enemy to face the player.
    private void LookAtTarget()
    {
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    // Commands the enemy to chase the player.
    private void ChasePlayer()
    {
        navAgent.SetDestination(player.position);
        navAgent.speed = (attackSpeed * 10) - 2;
        animator.SetFloat("Speed", attackSpeed);
        navAgent.isStopped = false;
    }

    // Attacks the player if in range.
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

    // Resets the attack after a set delay.
    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Shooting", playerInAttackRange);
    }

    // Deals damage to the enemy and plays a hit effect.
    public void TakeDamage(float damage)
    {
        if (health <= 0) return; // Prevent extra damage calls after death

        health -= damage;
        hitEffect.Play();

        if (health <= 0)
        {
            Die();
        }
        else if (!takeDamage) // Avoid restarting coroutine if already taking damage
        {
            StartCoroutine(TakeDamageCoroutine());
        }
    }

    // Coroutine to handle the damage state for the enemy.
    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f);
        takeDamage = false;
    }

    // Handles the death of the enemy.
    private void Die()
    {
        takeDamage = false;
        Destroy(gameObject, 0.5f);
        levelComplete.numEnemiesDestroyed++;
        Debug.Log("Enemies Killed: " + levelComplete.numEnemiesDestroyed);
    }

    // Draws gizmos in the scene to visualize the attack and sight range.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
