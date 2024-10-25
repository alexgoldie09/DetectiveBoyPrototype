using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy the new one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Assign this as the instance
            instance = this;
        }
    }

    [Header("Puzzle Canvas")]
    [SerializeField] private GameObject canvas; // Reference to canvas UI

    private void Start()
    {
        canvas.SetActive(false);
    }

    private void Update()
    {
        // Check for the "I" key input to toggle the Canvas
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleKeypad();
        }
    }

    // Toggles the Canvas active state
    private void ToggleKeypad()
    {
        bool isActive = canvas.activeSelf;
        if(isActive)
        {
            // Disable cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Enable cursor
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        canvas.SetActive(!isActive);
    }
}
