using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class updates the camera's position to match a target position every frame.
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Updates the camera's position every frame to match the target position.
    private void Update()
    {
        transform.position = cameraPosition.position; // Set the camera's position to match the target.
    }
}