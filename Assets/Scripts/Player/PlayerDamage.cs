using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the player's health, damage, death, and game over behavior.
public class PlayerDamage : MonoBehaviour
{
    public int health = 100;
    public PlayerMovement playerMove;
    public bool Dead;
    Rigidbody playerRB;
    Sliding sliding;
    AudioSource audioSource;
    [SerializeField] private GameOverUI gameOver;
    [SerializeField] private Camera cam;
    public float camFOVIn;
    [SerializeField] private float camFOVOut;
    [HideInInspector] public float countdown = 0f;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private PickUpSystem pickUp1;
    [SerializeField] private PickUpSystem pickUp2;

    // This method is called when the player takes damage.
    public void TakeDamage(int damage)
    {
        health -= damage; // Subtract health by damage amount.
        if (health <= 0)
        {
            pickUp1.enabled = false; // Disable pickups.
            pickUp2.enabled = false;
            gameOverScreen.SetActive(true); // Show the game over screen.
            StartCoroutine(PlayerDie()); // Start the player's death process.
        }
    }

    // This coroutine handles the player's death sequence.
    public IEnumerator PlayerDie()
    {
        Dead = true; // Set the player's status to dead.
        yield return null; // Wait a frame to continue execution.

        playerMove.Die(); // Call the player's movement die method.
        sliding.Die(); // Call the sliding die method.

        float elapsed = 0; // Timer to handle the camera FOV change.
        countdown = camFOVIn + 1; // Set the countdown for FOV transition.

        // Gradually change the camera FOV to simulate a death effect.
        while (elapsed < camFOVIn)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 135f, elapsed / camFOVIn);
            elapsed += Time.deltaTime; // Increment time.
            gameOver.Update(); // Update the game over UI.
            yield return null; // Wait for the next frame.
        }
    }
}
