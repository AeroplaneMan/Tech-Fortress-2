using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the player's camera and mouse look functionality.
public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRoatation;

    [SerializeField] private PlayerDamage playerDamage;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private LevelCompleteCheck levelComplete;
    [SerializeField] private PauseMenu pause;

    // This method is called when the script is first initialized.
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        Time.timeScale = 1.0f; // Set the game speed to normal
        playerMovement.Respawn(); // Ensure the player is respawned at the start
    }

    // This method is called every frame.
    private void Update()
    {
        if (playerDamage.Dead || levelComplete.levelCompleted || pause.isPaused)
        {
            // If the player is dead, the level is complete, or the game is paused, show the cursor and unlock it.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Otherwise, lock the cursor and hide it for mouse input.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Get mouse input for camera rotation.
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRoatation += mouseX; // Rotate the camera on the Y-axis.

            xRotation -= mouseY; // Rotate the camera on the X-axis.
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit the camera's vertical rotation.

            // Apply the rotation to the camera and the orientation.
            transform.rotation = Quaternion.Euler(xRotation, yRoatation, 0);
            orientation.rotation = Quaternion.Euler(0, yRoatation, 0);
        }
    }
}
