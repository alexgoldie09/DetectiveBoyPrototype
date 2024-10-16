using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public static PlayerController instance { get; private set; }

    //private void Awake()
    //{
    //    // If an instance already exists and it's not this one, destroy the new one
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        // Assign this as the instance
    //        instance = this;

    //        // Optionally, ensure this object persists across scenes
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}

    [Header("References")]
    [SerializeField] private Transform orientation; // Reference to which way the player is orientated
    [SerializeField] private Rigidbody rb; // Reference to player's rigidbody
    [SerializeField] private Animator anim; // Reference to player's animator

    [Header("Movement variables")]
    [SerializeField] private float moveSpeed = 2f; // Reference to player move speed
    [SerializeField] private float interpolation = 10f; // Rate of interpolation
    [SerializeField] private float walkScale = 0.33f; // Reference to slow down to walk
    private float inputX, inputY; // Reference to movement variables
    private Vector3 moveDir; // Reference to move direction

    [Header("Jump variables")]
    [SerializeField] private float jumpForce = 8; // Reference to player jump force
    [SerializeField] private float jumpCooldown = 0.5f; // Reference to jump cooldown
    private bool readyToJump = true; // Reference to whether can jump again

    [Header("Ground check variables")]
    [SerializeField] private Transform groundCheck; // Assign a transform slightly below the player
    [SerializeField] private float groundCheckDistance = 0.2f; // How far to check below player
    [SerializeField] private LayerMask groundMask; // Assign a ground layer mask
    [SerializeField] private float groundDrag; // Add drag resistance to movement

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Check if the player is in conversation
        if (!Extensions.isTalking && !Extensions.isExamining)
        {
            // Call move player function
            MovePlayer();
        }
        else
        {
            // Set velocity to zero
            rb.velocity = Vector3.zero;
            // Set move direction to zero
            moveDir = Vector3.zero;
            // Set animation to idle
            anim.SetFloat("MoveSpeed", 0);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Set animator to be grounded
        anim.SetBool("Grounded", IsGrounded());

        // Handle drag
        if(IsGrounded())
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        // Call OnClick
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OnClick();
        //}
    }

    // Function for accepting player input
    private void MyInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Check when to jump
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && IsGrounded())
        {
            Jump();

            StartCoroutine(ResetJump());
        }

        // Factor for increase movement speed
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            inputY *= walkScale;
            inputX *= walkScale;
        }

        inputY = Mathf.Lerp(inputY, y, Time.deltaTime * interpolation);
        inputX = Mathf.Lerp(inputX, x, Time.deltaTime * interpolation);
    }

    // Function for moving player
    private void MovePlayer()
    {
        // Call input function
        MyInput();

        // Call speed control
        SpeedControl();

        // Calculate movement direction
        Vector3 currentDir = orientation.forward * inputY + orientation.right * inputX;

        float directionLength = currentDir.magnitude;
        currentDir.y = 0;
        currentDir = currentDir.normalized * directionLength;

        // Move player using physics
        if (currentDir != Vector3.zero)
        {
            moveDir = Vector3.Slerp(moveDir, currentDir, Time.deltaTime * interpolation);

            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
            // Set animator
            anim.SetFloat("MoveSpeed", currentDir.magnitude);
        }
    }

    // Function for controlling speed
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x,0f, rb.velocity.z);

        // Limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Funtion for jumping
    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Function for reset jump
    private IEnumerator ResetJump()
    {
        readyToJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        readyToJump = true;
    }

    // Function for OnClick
    //private void OnClick()
    //{
    //    RaycastHit hit;
    //    Ray camToScreen = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(camToScreen, out hit, Mathf.Infinity))
    //    {
    //        if (hit.collider != null)
    //        {
    //            Interactable interactable = hit.collider.GetComponent<Interactable>();

    //            if (interactable != null)
    //            {
    //                interactable.Interact(this);
    //            }
    //        }
    //    }
    //}

    // Function for ground check
    private bool IsGrounded() => Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundMask);
}
