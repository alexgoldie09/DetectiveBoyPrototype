using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    //[Header("Jump variables")]
    //[SerializeField] private float jumpForce = 8; // Reference to player jump force
    //[SerializeField] private float jumpCooldown = 0.5f; // Reference to jump cooldown
    //private bool readyToJump = true; // Reference to whether can jump again

    [Header("Ground check variables")]
    [SerializeField] private Transform groundCheck; // Assign a transform slightly below the player
    [SerializeField] private float groundCheckDistance = 0.2f; // How far to check below player
    [SerializeField] private LayerMask groundMask; // Assign a ground layer mask
    [SerializeField] private float groundDrag; // Add drag resistance to movement

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
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

        if(Extensions.isTalking)
        {
            anim.SetBool("isTalking", true);
        }
        else
        {
            anim.SetBool("isTalking", false);
        }

        // Call OnClick
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OnClick();
        //}
    }

    private void MyInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Factor for increased movement speed (walking or running)
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            x *= walkScale;
            y *= walkScale;
        }

        // Dynamic interpolation for quicker zero reset
        float lerpSpeed = Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0 ? interpolation : interpolation * 2;

        inputY = Mathf.Lerp(inputY, y, Time.deltaTime * lerpSpeed);
        inputX = Mathf.Lerp(inputX, x, Time.deltaTime * lerpSpeed);

        // Ensure small values snap to zero
        if (Mathf.Abs(inputY) < 0.01f) inputY = 0;
        if (Mathf.Abs(inputX) < 0.01f) inputX = 0;
    }

    private void MovePlayer()
    {
        // Call input function
        MyInput();

        // Calculate movement direction
        Vector3 currentDir = orientation.forward * inputY + orientation.right * inputX;

        // Normalize to prevent diagonal speed increase
        if (currentDir.magnitude > 1)
        {
            currentDir = currentDir.normalized;
        }

        // Apply walkScale only when Left Shift is pressed
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            currentDir *= walkScale;
        }

        if (currentDir != Vector3.zero)
        {
            // Smoothly adjust movement direction
            moveDir = Vector3.Slerp(moveDir, currentDir, Time.deltaTime * interpolation);

            // Apply movement force
            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);

            // Update animation with movement speed
            anim.SetFloat("MoveSpeed", currentDir.magnitude);
        }
        else
        {
            // Reset MoveSpeed to 0 when idle
            moveDir = Vector3.zero;
            anim.SetFloat("MoveSpeed", 0);
        }

        // Control the speed to ensure it doesn't exceed moveSpeed
        SpeedControl();
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
    //private void Jump()
    //{
    //    // Reset y velocity
    //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    //}

    // Function for reset jump
    //private IEnumerator ResetJump()
    //{
    //    readyToJump = false;
    //    yield return new WaitForSeconds(jumpCooldown);
    //    readyToJump = true;
    //}

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
