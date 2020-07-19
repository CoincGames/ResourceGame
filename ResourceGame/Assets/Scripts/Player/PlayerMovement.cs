using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The character controller that is in charge of the player.")]
    private CharacterController controller = null;

    [Header("Properties")]

    [SerializeField]
    [Tooltip("The desired move speed of the character from a walk.")]
    [Range(1f, 100f)]
    private float moveSpeed = 5f;

    public bool isSprinting = false;
    public float sprintMultiplier = 2f;

    [SerializeField]
    [Tooltip("The force of gravity applied to the player per second")]
    private float gravity = -9.81f;

    [SerializeField]
    [Tooltip("The jump height of the player.")]
    private float jumpHeight = 0;
    
    [Header("Player On Ground Tools")]

    [SerializeField]
    [Tooltip("The game object that will check if the player is standing on ground.\n\nNOTE: Should be on the bottom of the player model.")]
    private Transform groundChecker = null;

    [SerializeField]
    [Tooltip("The distance below the player that the hit detection checks.\n\nNOTE: This is a radius of a generated sphere.")]
    private float groundDistance = .2f;

    [SerializeField]
    [Tooltip("The layer for which the ground checker is searching for.\n\nNOTE: Anything that should be stood on, should be of part Terrain.")]
    public LayerMask groundLayer;

    public bool isMovingForwardBackward;
    public bool isMovingLeftRight;

    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        checkIfGrounded();
        doPlayerMovement();
        applyGravity();
        ListenForSprint();
    }

    private void checkIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void doPlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        isMovingForwardBackward = z != 0;
        isMovingLeftRight = x != 0;

        Vector3 moveVector = transform.right * x + transform.forward * z;

        controller.Move(moveVector * moveSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void applyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void ListenForSprint()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = isSprinting ? moveSpeed / sprintMultiplier : moveSpeed * sprintMultiplier;

            isSprinting = !isSprinting;
        }
    }
}
