using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles spawning enemies in the game at random positions within a defined area.
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minZPos;
    [SerializeField] private float maxZPos;
    [SerializeField] private float spawnRadius;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LevelCompleteCheck levelComplete;
    private int numEnemiesCreated;

    // Starts the enemy spawning process when the game begins.
    void Start()
    {
        StartCoroutine(spawnEnemy(spawnInterval, enemyPrefab));
    }

    // Coroutine that spawns enemies at defined intervals and ensures they spawn in valid positions.
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        // Keeps spawning enemies until the enemy limit is reached.
        while (numEnemiesCreated < levelComplete.enemyLimit)
        {
            yield return new WaitForSeconds(interval);
            Vector3 spawnPosition;
            bool foundValidSpawn = false;

            // Tries to find a valid spawn position by checking for obstacles.
            while (!foundValidSpawn)
            {
                spawnPosition = new Vector3(Random.Range(minXPos, maxXPos), 0, Random.Range(minZPos, maxZPos));

                // Visualizes the raycast in the editor for debugging purposes.
                Debug.DrawRay(spawnPosition + Vector3.up * 1f, Vector3.down * 5f, Color.red, 5f);

                // Checks for obstacles within the spawn radius to ensure a valid spawn position.
                if (Physics.OverlapSphere(spawnPosition, spawnRadius, obstacleLayer).Length == 0)
                {
                    foundValidSpawn = true;
                    // Instantiates the enemy at the valid spawn position.
                    GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
                    numEnemiesCreated++;
                    newEnemy.SetActive(true);
                    Debug.Log("Number of enemies: " + numEnemiesCreated);
                }
            }

            GetNumEnemiesCreated();
        }
    }

    // Method to get the number of enemies created.
    public int GetNumEnemiesCreated()
    {
        return numEnemiesCreated;
    }
}
