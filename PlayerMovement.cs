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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // was es schauen soll bei gravity

        
  

        if (isGrounded && velocity.y < 0) // checkt position und macht das keine Gravity sein soll
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y; // berechnet wohin wir uns drehen müssen
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTime); // smoothed die drehen (targetAngle)
            transform.rotation = Quaternion.Euler(0f , angle, 0f); // drehen

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // bewegt sich von der kamera nach vorne
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);  // bewegen
        }

        if (Input.GetButtonDown("Sprint") && isGrounded) // macht das sprint nur aktiv ist wenn man Button runter hat.
        {
            speed = 15f;
        }

        if (Input.GetButtonUp("Sprint"))
        {
            speed = 6f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2.0f * gravity); // jumpt
        }

        if(Input.GetButtonDown("Dash"))
        {
            
        }

        velocity.y += gravity * Time.deltaTime; // pullt den character down
        controller.Move(velocity * Time.deltaTime);
    }
}
