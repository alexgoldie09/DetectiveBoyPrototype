using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueHover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCam; // Reference to the main camera
    [SerializeField] private LayerMask clueLayer;          // Layer mask for clue objects

    private Clue currentClue = null;            // Reference to the currently hovered clue object


    private void Start()
    {
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    private void Update()
    {
        if (Extensions.isExamining)
        {
            DetectClueHover();

            // Check if a clue is currently hovered and if the F key is pressed
            if (currentClue != null && currentClue.ItemProducedAction != null && Input.GetKeyDown(KeyCode.F))
            {
                OnClueInteract();
            }
        }
        else
        {
            currentClue?.HideClueUI();
            currentClue = null;
        }
    }

    // Method to detect if the mouse is hovering over a clue object
    private void DetectClueHover()
    {
        // Cast a ray from the mouse position into the scene
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast and check if it hits an object on the clue layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clueLayer))
        {
            // Check if the object hit has a Clue component and is a different clue by ID
            Clue clue = hit.collider.GetComponent<Clue>();
            if (clue != null && (currentClue == null || currentClue.ClueId != clue.ClueId))
            {
                // Hide the previous clue's UI if there was one
                currentClue?.HideClueUI();

                // Update the current clue and show the new clue's UI
                currentClue = clue;
                currentClue.ShowClueUI();
            }
        }
        else
        {
            // Hide the current clue's UI if not hovering over any clue object
            currentClue?.HideClueUI();
            currentClue = null;
        }
    }

    // Method to handle interaction when F key is pressed
    private void OnClueInteract()
    {
        // Call a function here (e.g., show more details, collect the clue, etc.)
        Debug.Log("Interacting with Clue: " + currentClue.ClueId);
        currentClue.ItemProducedAction.Act();
        currentClue.gameObject.SetActive(false);
        // Example: You could call any function specific to the clue here
        // currentClue.PerformAction(); // Uncomment if `Clue` has such a function
    }
}
