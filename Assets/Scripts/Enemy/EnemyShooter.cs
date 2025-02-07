using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("General")]
    public Transform attackPoint;
    public Transform gunPoint;
    public LayerMask layerMask;
    public int playerDamage;
    private int numBullets;

    [Header("Gun")]
    public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);
    public TrailRenderer bulletTrail;
    private EnemyReferences enemyReferences;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    public void Shoot()
    {
        Vector3 direction = GetDirection();
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(attackPoint.position, direction, out hit, float.MaxValue, layerMask))
        {
            targetPoint = hit.point;

            PlayerDamage player = hit.collider.GetComponent<PlayerDamage>();
            if (player != null)
            {
                numBullets++;
                if (numBullets >= 3)
                {
                    Debug.Log("Hit the player!");
                    player.TakeDamage(playerDamage);
                    numBullets = 0;
                }  
            }

            Debug.DrawLine(attackPoint.position, attackPoint.position + direction * 10f, Color.red, 1f);

            TrailRenderer trail = Instantiate(bulletTrail, gunPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        direction += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
        );
        direction.Normalize();
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail.gameObject, trail.time);
    }
}
