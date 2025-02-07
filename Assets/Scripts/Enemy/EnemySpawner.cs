using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        StartCoroutine(spawnEnemy(spawnInterval, enemyPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        while (numEnemiesCreated < levelComplete.enemyLimit)
        {
            yield return new WaitForSeconds(interval);
            Vector3 spawnPosition;
            bool foundValidSpawn = false;

            while (!foundValidSpawn)
            {
                spawnPosition = new Vector3(Random.Range(minXPos, maxXPos), 0, Random.Range(minZPos, maxZPos));

                // Visualize the raycast in the editor
                Debug.DrawRay(spawnPosition + Vector3.up * 1f, Vector3.down * 5f, Color.red, 5f);

                // Check for collision with walls
                if (Physics.OverlapSphere(spawnPosition, spawnRadius, obstacleLayer).Length == 0)
                {
                    foundValidSpawn = true;
                    GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
                    numEnemiesCreated++;
                    newEnemy.SetActive(true);
                    Debug.Log("Number of enemies: " + numEnemiesCreated);
                }
            }

            GetNumEnemiesCreated();
        }
    }

    // Method to get the number of enemies created
    public int GetNumEnemiesCreated()
    {
        return numEnemiesCreated;
    }
}