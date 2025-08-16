using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public Transform camera;
    public Transform groundCheck;
    public LayerMask groundMask;
    private Vector3 velocity;
    
    private bool isGrounded;
    public float speed = 6f;
    public float smoothTime = 0.1f;
    public float smoothVelocity;
    public float gravity = -9.81f;
    public float groundDistance = 0.2f;
    public float jump = 1.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // checks if the player isGrounded

        
  

        if (isGrounded && velocity.y < 0) // checks position and then sets the gravity to a value
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTime); 
            transform.rotation = Quaternion.Euler(0f , angle, 0f); // turn

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);  // move
        }

        if (Input.GetButtonDown("Sprint") && isGrounded) // makes character move faster when a button is Pressed
        {
            speed = 15f;
        }

        if (Input.GetButtonUp("Sprint")) // stops the sprint when you let go of the is Pressed button
        {
            speed = 6f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2.0f * gravity); // jump
        }


        velocity.y += gravity * Time.deltaTime; // pullt den character down
        controller.Move(velocity * Time.deltaTime);
    }
}
