using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.Timeline;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public GameObject activeChar;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    bool isJumping = false;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(speed * Time.deltaTime * move);

        // Detect jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            isJumping = true;
            activeChar.GetComponent<Animator>().Play("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Movement detection
        if (isGrounded && move != Vector3.zero)
        {
            isMoving = true;

            // Determine direction
            if (z > 0)
            {
                speed = 12f;
                activeChar.GetComponent<Animator>().Play("Run_guard_AR");
            }
            else if (z < 0)
            {
                activeChar.GetComponent<Animator>().Play("WalkBack_Shoot_AR");
                speed = 8f;
            }
            else if (x > 0)
            {
                speed = 8f;
                activeChar.GetComponent<Animator>().Play("WalkRight_Shoot_AR");
            }
            else if (x < 0)
            {
                speed = 8f;
                activeChar.GetComponent<Animator>().Play("WalkLeft_Shoot_AR");
            }
        }
        else
        {
            isMoving = false;
        }
        if (isGrounded == true && isMoving == false && isJumping == false) activeChar.GetComponent<Animator>().Play("Idle_Guard_AR");

        lastPosition = gameObject.transform.position;
        if (isGrounded)
        {
            isJumping = false;
        }
    }

}
