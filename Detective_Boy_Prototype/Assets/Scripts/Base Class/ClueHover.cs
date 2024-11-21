using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueHover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCam; // Reference to the main camera
    [SerializeField] private LayerMask clueLayer;          // Layer mask for clue objects

    private Clue currentClue;            // Reference to the currently hovered clue object


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

            // If a clue is currently hovered, check for F key press to interact
            if (currentClue != null && currentClue.ItemProducedAction != null && Input.GetKeyDown(KeyCode.F))
            {
                OnClueInteract();
            }
        }
        else
        {
            // Hide the current clue's UI if not hovering over any clue object
            currentClue?.HideClueUI();
            currentClue = null;
        }
    }

    // Method to detect if the player is hovering over a clue
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
        Debug.Log("Interacting with Clue: " + currentClue.ClueId);
        currentClue.ItemProducedAction.Act();
        StartCoroutine(DelayedDeactivation(currentClue.gameObject, false, 0.1f));
        // You could add more functionality here, e.g., collecting or examining the clue
    }

    private IEnumerator DelayedDeactivation(GameObject target, bool isActive, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Set the active state
        target.SetActive(isActive);
    }
}
