using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelCompleteCheck : MonoBehaviour
{
    [HideInInspector] public int numEnemiesDestroyed;
    public int enemyLimit;
    [HideInInspector] public bool levelCompleted = false;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private PickUpSystem pickUp1;
    [SerializeField] private PickUpSystem pickUp2;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Sliding sliding;

    void Update()
    {
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        int totalEnemiesCreated = enemySpawner.GetNumEnemiesCreated();
        if (numEnemiesDestroyed >= totalEnemiesCreated && totalEnemiesCreated >= enemyLimit)
        {
            StartCoroutine(LevelComplete());
        }
    }

    private IEnumerator LevelComplete()
    {
        yield return  new WaitForSeconds(0.5f);
        levelCompleted = true;
        pickUp1.enabled = false;
        pickUp2.enabled = false;
        playerMovement.Die();
        sliding.Die();
        Time.timeScale = 0f;
        levelCompleteScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT!");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0); // Load the first scene (Main Menu)
    }
}

//public class LevelCompleteCheck2 : MonoBehaviour
//{
//    public int enemyLimit; // Total number of enemies for the level
//    public int numEnemiesDestroyed; // Number of enemies destroyed
//    public EnemySpawner enemySpawner; // Reference to the spawner

//    private void Update()
//    {
//        CheckLevelComplete();
//    }

//    private void CheckLevelComplete()
//    {
//        int totalEnemiesCreated = enemySpawner.GetNumEnemiesCreated();

//        // Check if all enemies have been spawned and destroyed
//        if (numEnemiesDestroyed >= totalEnemiesCreated && totalEnemiesCreated >= enemyLimit)
//        {
//            Debug.Log("Level Complete!");
//            EndGame();
//        }
//    }

//    private void EndGame()
//    {
//        // Handle game end logic (e.g., show a level complete UI, stop spawners, etc.)
//        Debug.Log("You finished the level!");
//    }
//}
