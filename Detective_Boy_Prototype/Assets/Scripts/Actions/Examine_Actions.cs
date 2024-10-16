using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Examine_Actions : Actions
{
    [Header("Variables")]
    [SerializeField] private CinemachineFreeLook thirdPersonCam; // Reference to third person cam
    [SerializeField] private CinemachineVirtualCamera firstPersonCam; // Reference to first person cam
    [SerializeField] private Transform itemHolder; // Empty GameObject in front of the camera for item positioning
    [SerializeField] private float rotSpeed = 100f;  // Speed of rotation

    private bool isDragging = false;   // To track if the player is currently dragging the item
    private Vector3 origPos; // Reference to original position
    private Quaternion origRot; //  Reference to original rotation
    private Rigidbody rb; // Reference to rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void Act()
    {
        Extensions.isExamining = true;
        origPos = transform.position;
        origRot = transform.rotation;
        thirdPersonCam.Priority = 0;  // Lower priority to switch out third person cam
        firstPersonCam.gameObject.SetActive(true); // Activate cam
        firstPersonCam.Priority = 10; // Raise priority to switch to first person view
        StartCoroutine(ExamineItem());
    }

    private IEnumerator ExamineItem()
    {
        // Move the item to the itemHolder's position, relative to the first-person camera
        transform.SetParent(itemHolder);
        transform.localPosition = Vector3.zero;  // Ensure it's centered at itemHolder
        transform.localRotation = Quaternion.identity;  // Reset rotation

        // Optionally disable the item's Rigidbody during examination
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Grab camera reference
        Camera mainCam = Camera.main;

        // Enable cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        while (Extensions.isExamining)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Optionally disable the item's Rigidbody during examination
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                transform.SetParent(null);
                transform.localRotation = origRot; // Ensure rotation is back to original
                transform.localPosition = origPos;  // Ensure is placed back

                // Return to third-person FreeLook camera
                firstPersonCam.Priority = 0; // Deactivate first-person cam
                firstPersonCam.gameObject.SetActive(false); // Activate cam
                thirdPersonCam.Priority = 10; // Reactivate FreeLook cam

                // Disable cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Extensions.isExamining = false;
            }
            else
            {
                // Call function to rotate
                HandleMouseInput();
            }
            yield return null;
        }
    }
        // Handle mouse input for click-and-drag rotation
        private void HandleMouseInput()
    {
        // Start rotating if the mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        // Stop rotating when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Rotate the item only while dragging
        if (isDragging)
        {
            RotateItemWithMouse();
        }
    }

    // Rotate the item based on mouse input
    private void RotateItemWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

        // Rotate the item around its local Y-axis (horizontal movement) and X-axis (vertical movement)
        transform.Rotate(Vector3.up, -mouseX, Space.World);  // Rotate around the Y-axis (world space)
        transform.Rotate(Vector3.right, mouseY, Space.World);  // Rotate around the X-axis (world space)
    }

}
