using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation; // Reference to which way the player is orientated
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private Rigidbody rb; // Reference to player's rigidbody

    [Header("Movement variables")]
    [SerializeField] private float rotSpeed; // Reference to camera rotation speed

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        // Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x,player.position.y,transform.position.z);
        // Set orientation to the view direction
        orientation.forward = viewDir.normalized;

        // Rotate player
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * inputY + orientation.right * inputX;

        if(inputDir != Vector3.zero)
        {
            player.forward = Vector3.Slerp(player.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
        }
    }

}
