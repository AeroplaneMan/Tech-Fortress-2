using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Handles level completion, checking if enough enemies have been defeated.
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

    // Called every frame to check if the level is completed.
    void Update()
    {
        CheckLevelComplete(); // Checks if the player has completed the level.
    }

    // Checks if the player has defeated enough enemies to complete the level.
    private void CheckLevelComplete()
    {
        int totalEnemiesCreated = enemySpawner.GetNumEnemiesCreated(); // Get the number of enemies spawned.
        if (numEnemiesDestroyed >= totalEnemiesCreated && totalEnemiesCreated >= enemyLimit)
        {
            StartCoroutine(LevelComplete()); // Start the level completion sequence if conditions are met.
        }
    }

    // Handles the level completion process, including disabling player actions and showing the level complete UI.
    private IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(0.5f); // Wait before triggering level completion.
        levelCompleted = true; // Mark the level as completed.
        pickUp1.enabled = false; // Disable pick-ups.
        pickUp2.enabled = false;
        playerMovement.Die(); // Disable player movement.
        sliding.Die(); // Disable sliding.
        Time.timeScale = 0f; // Pause the game.
        levelCompleteScreen.SetActive(true); // Show the level complete screen.
    }

    // Quits the game application.
    public void Quit()
    {
        Application.Quit(); // Exit the game.
        Debug.Log("QUIT!"); // Log the quit action.
    }

    // Loads the main menu scene (scene index 0).
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0); // Load the main menu scene.
    }
}

//public class LevelCompleteCheck2 : MonoBehaviour IGNORE THIS STUFF, OLD CLASS
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
