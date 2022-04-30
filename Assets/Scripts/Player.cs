using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerVelocity = 1.4f;
    public float jumpForce = 1f;
    public float gravity = -9.81f;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;

    private CharacterController _controller;
    public Vector3 moveVector;
    private Animator animator;
    public bool _isGrounded = true;
    public bool isJumping;
    private Transform _groundChecker;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(2);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        // Check if Player is grounded and stop gravity action
        if (_isGrounded && moveVector.y < 0)
        {
            moveVector.y = 0f;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            isJumping = true;
        }

    }

    void FixedUpdate()
    {
        // Horizontal Movement
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), _controller.velocity.y, 0);
        _controller.Move(playerVelocity * Time.deltaTime * moveVector);

        // Send current speed to Speed Animator variable
        animator.SetFloat("Speed", _controller.velocity.x);

        // Player Facing Direction
        if (moveVector.x < -0.01)
        {
            transform.forward = new Vector3(-90, 0, 0);
        }
        else if (moveVector.x > 0.01)
        {
            transform.forward = new Vector3(90, 0, 0);
        }

        // Jumping
        if (isJumping)
        {
            Debug.Log("JUMPING");
            moveVector.y += Mathf.Sqrt(jumpForce * -2.0f * gravity);
            isJumping = false;
        }

        moveVector.y += gravity * Time.deltaTime;
        _controller.Move(moveVector * Time.deltaTime);      
    }
}
