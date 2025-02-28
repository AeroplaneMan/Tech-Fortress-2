using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages the player's sliding mechanic, including movement, timer, and scaling.
public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    // Initializes the references and stores the player's initial Y scale.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    // Checks for player input and handles sliding activation or stopping.
    private void Update()
    {
        if (pm.state == PlayerMovement.MovementState.dead)
        {
            horizontalInput = 0;
            verticalInput = 0;
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Initiates slide if the slide key is pressed and there is movement input.
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        // Stops the slide if the slide key is released.
        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();
    }

    // Handles sliding movement physics.
    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    // Starts the sliding process: sets scaling and applies initial forces.
    private void StartSlide()
    {
        pm.sliding = true;

        // Adjusts player scale for the slide.
        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // Applies downward force.
        slideTimer = maxSlideTime; // Initializes the slide timer.
    }

    // Controls the movement during the slide.
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Handles sliding on flat ground or small slopes.
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        // Handles sliding down steeper slopes.
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        // Stops the slide if the slide timer runs out.
        if (slideTimer <= 0)
            StopSlide();
    }

    // Stops the slide and restores the player's original scale.
    private void StopSlide()
    {
        pm.sliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }

    // Stops the slide when the player dies.
    public void Die()
    {
        StopSlide();
    }
}
