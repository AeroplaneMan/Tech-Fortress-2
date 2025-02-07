using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerDamage player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PickUpSystem pickUp1;
    [SerializeField] private PickUpSystem pickUp2;
    [SerializeField] private AudioClip gameOverAudioClip;
    public TMP_Text gameOverText;

    private bool hasPlayedGameOverAudio = false;

    public void Update()
    {
        if (player.Dead)
        {
            gameOverText.text = "Game Over!";

            // Freeze the game and play the game-over audio only once
            if (!hasPlayedGameOverAudio)
            {
                hasPlayedGameOverAudio = true; // Set the flag first
                Time.timeScale = 0f; // Freeze the game
                StartCoroutine(PlayGameOverAudio());
            }
        }
        else
        {
            gameOverText.text = "Health: " + (player.health / 2) + "%";
            hasPlayedGameOverAudio = false; // Reset the flag if player is not dead
        }
    }

    private IEnumerator PlayGameOverAudio()
    {
        audioSource.Stop(); // Stop the current clip
        yield return null; // Wait for one frame to ensure the audio stops
        audioSource.clip = gameOverAudioClip; // Set the new clip
        audioSource.Play(); // Play the new clip
    }

    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pickUp1.enabled = true;
        pickUp2.enabled = true;
        Debug.Log("QUIT!");
    }
}