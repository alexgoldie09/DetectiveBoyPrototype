using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueHover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCam; // Reference to the main camera
    [SerializeField] private LayerMask clueLayer;          // Layer mask for clue objects

    [Header("Clue UI")]
    [SerializeField] private GameObject clueUIPanel;  // The UI panel that shows the clue
    [SerializeField] private TextMeshProUGUI clueDescriptionText;  // The Text component to display the clue description

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
        }
        else
        {
            // Hide the UI if not hovering over any clue object
            HideClueUI();
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
            Clue clue = hit.collider.GetComponent<Clue>();

            if (clue != null)
            {
                // If we're hovering over a clue object, show the clue UI and set the text
                currentClue = clue;
                ShowClueUI(currentClue.Description);

                if(Input.GetKeyDown(KeyCode.F))
                {
                    currentClue.ItemProducedAction.Act();
                    currentClue.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // Hide the UI if not hovering over any clue object
            HideClueUI();
            currentClue = null;
        }
    }

    // Method to display the clue UI with the description
    private void ShowClueUI(string description)
    {
        clueUIPanel.SetActive(true);
        clueDescriptionText.text = description;
    }

    // Method to hide the clue UI
    private void HideClueUI()
    {
        clueUIPanel.SetActive(false);
    }
}
