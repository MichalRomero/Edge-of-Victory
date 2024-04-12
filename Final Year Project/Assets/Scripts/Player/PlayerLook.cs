using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam; // Reference to the camera attached to the player.
    private float xRotation = 0f;

    public float xSens = 20f; // Sensitivity for horizontal camera movement.
    public float ySens = 20f; // Sensitivity for vertical camera movement.

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Calculate the rotation for looking up and down.
        xRotation -= (mouseY * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Clamp vertical rotation to avoid over-rotation.

        // Create a quaternion for the vertical rotation.
        Quaternion verticalRotation = Quaternion.Euler(xRotation, 0, 0);

        // Apply the rotation to the camera's transform.
        cam.transform.localRotation = verticalRotation;

        // Rotate the player to look left and right.
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSens);
    }
}


