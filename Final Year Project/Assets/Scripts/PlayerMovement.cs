using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpforce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        //moves in any given direction with set movement speed
        rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, VerticalInput * movementSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpforce, rb.velocity.z);
        }

    }
}
