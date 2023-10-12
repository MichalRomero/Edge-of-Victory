using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam; // Reference to the camera attached to the player.
    private float xRotation = 0f;

    public float xSensitivity = 30f; // Sensitivity for horizontal camera movement.
    public float ySensitivity = 30f; // Sensitivity for vertical camera movement.

    public void ProcessLook(Vector2 input){
        float mouseX = input.x;
        float mouseY = input.y;

        // Calculate the camera rotation for looking up and down.
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Clamp vertical rotation to avoid over-rotation.

        // Apply the rotation to the camera's transform.
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate the player to look left and right.
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
