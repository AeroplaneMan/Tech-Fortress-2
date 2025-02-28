using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CustomBullet handles the behavior of bullets, including collision, explosions, and damage application.
public class CustomBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsPlayer;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public int explosionDamage;
    public float explosionRange;

    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch;

    int collisions;
    PhysicMaterial physics_mat;

    // Initializes bullet properties when the object is created.
    private void Start()
    {
        Setup();
    }

    // Updates bullet state, checking for collisions or lifetime expiration.
    private void Update()
    {
        if (collisions > maxCollisions) Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    // Triggers an explosion, dealing damage to enemies and the player within range.
    private void Explode()
    {
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        // Damage enemies within explosion range
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyAI enemyAI = enemies[i].GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(explosionDamage);
            }
        }

        // Damage player within explosion range
        Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, whatIsPlayer);
        for (int i = 0; i < players.Length; i++)
        {
            PlayerDamage playerHealth = players[i].GetComponent<PlayerDamage>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
                Debug.Log("Player hit by explosion! Damage: " + explosionDamage);
            }
        }

        Invoke("Delay", 0.05f);
    }

    // Destroys the bullet object after a short delay.
    private void Delay()
    {
        Destroy(gameObject);
    }

    // Handles collision logic, tracking the number of impacts and triggering explosions if necessary.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet")) return;

        collisions++;

        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
            Explode(); Debug.Log("BOOM");
    }

    // Sets up physics properties like bounciness and gravity when the bullet is initialized.
    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }

    // Visualizes the explosion radius in the Unity editor for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
