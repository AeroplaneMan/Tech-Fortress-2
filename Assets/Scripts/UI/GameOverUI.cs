using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Manages the game over UI, audio, and freezing the game when the player dies.
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerDamage player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PickUpSystem pickUp1;
    [SerializeField] private PickUpSystem pickUp2;
    [SerializeField] private AudioClip gameOverAudioClip;
    public TMP_Text gameOverText;

    private bool hasPlayedGameOverAudio = false;

    // Called every frame to update the game-over status based on player health.
    public void Update()
    {
        if (player.Dead)
        {
            gameOverText.text = "Game Over!"; // Display the game over text.

            // Freeze the game and play the game-over audio only once
            if (!hasPlayedGameOverAudio)
            {
                hasPlayedGameOverAudio = true; // Set the flag first
                Time.timeScale = 0f; // Freeze the game
                StartCoroutine(PlayGameOverAudio()); // Play the game over audio.
            }
        }
        else
        {
            gameOverText.text = "Health: " + (player.health / 2) + "%"; // Display the player's current health.
            hasPlayedGameOverAudio = false; // Reset the flag if player is not dead.
        }
    }

    // Plays the game-over audio clip.
    private IEnumerator PlayGameOverAudio()
    {
        audioSource.Stop(); // Stop the current audio clip.
        yield return null; // Wait for one frame to ensure the audio stops.
        audioSource.clip = gameOverAudioClip; // Set the new game over audio clip.
        audioSource.Play(); // Play the game over audio clip.
    }

    // Quits the game and reloads the current scene.
    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene.
        pickUp1.enabled = true; // Re-enable pick-ups.
        pickUp2.enabled = true;
        Debug.Log("QUIT!"); // Log the quit action.
    }
}
