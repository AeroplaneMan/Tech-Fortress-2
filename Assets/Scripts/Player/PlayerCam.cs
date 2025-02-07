using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        playerMovement.Respawn();
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerDamage.Dead || levelComplete.levelCompleted || pause.isPaused) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRoatation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRoatation, 0);
            orientation.rotation = Quaternion.Euler(0, yRoatation, 0);
        }
    }
}
