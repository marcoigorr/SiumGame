using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 1f;
    public float gravity = -9.81f;
    public float GroundDistance = 0.2f;
    private Transform _groundChecker;
    public LayerMask Ground;
    public bool _isGrounded = false;
    public bool isJumping;

    private CharacterController _controller;
    private float inputMovement;
    public float playerSpeed = 1.6f;
    public Vector3 moveVector;
    private Animator animator;

    public Transform targetTransform;

    private Camera mainCamera;
    public LayerMask mouseAimMask;

    private int FacingSign
    {
        get
        {
            Vector3 perp = Vector3.Cross(transform.forward, Vector3.forward);
            float dir = Vector3.Dot(perp, transform.up);
            return dir > 0 ? -1 : dir < 0 ? 1 : 0;
        }
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(2);
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        animator.SetBool("isGrounded", _isGrounded);

        inputMovement = Input.GetAxis("Horizontal");

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            isJumping = true;
        }

        // Move targetTransform to mouse position using a layermask
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
        }

    }

    void FixedUpdate()
    {
        // Horizontal Movement
        Vector3 moveVector = new Vector3(inputMovement, _controller.velocity.y, 0);
        _controller.Move(playerSpeed * Time.deltaTime * moveVector);
        // Send current speed to Speed Animator variable
        animator.SetFloat("Speed", (FacingSign * _controller.velocity.x) / playerSpeed);

        // Player Facing Direction
        transform.rotation = Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTransform.position.x - transform.position.x), 0)); 

        // Jumping
        if (isJumping)
        {
            Debug.Log("JUMP");
            moveVector.y += Mathf.Sqrt(jumpForce * -2.0f * gravity);
            isJumping = false;
        }

        moveVector.y += gravity * Time.deltaTime;
        _controller.Move(moveVector * Time.deltaTime);      
    }

    private void OnAnimatorIK()
    {
        // Weapon Aim at target IK
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, targetTransform.position);

        // Look at target IK
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(targetTransform.position);

    }
}
