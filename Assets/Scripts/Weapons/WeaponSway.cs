using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles weapon sway based on mouse movement, creating a smoother FPS aiming experience.
public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth; // Determines how smooth the sway effect is
    [SerializeField] private float swayMultiplier; // Controls the intensity of the sway
    public Quaternion SwayRotation { get; private set; } // Stores the calculated sway rotation

    private void Update()
    {
        // Get raw mouse input values
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        // Calculate rotation based on mouse movement
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        // Combine rotations to get the final target rotation
        Quaternion targetRotation = rotationX * rotationY;

        // Smoothly interpolate towards the target rotation
        SwayRotation = Quaternion.Slerp(SwayRotation, targetRotation, smooth * Time.deltaTime);
    }
}
