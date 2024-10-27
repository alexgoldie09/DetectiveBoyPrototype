using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [Header("Clue Info")]
    [SerializeField] private int clueId;  // Reference to Id number
    [TextArea]
    [SerializeField] private string description;  // This is the description that will be shown in the UI

    [Header("Clue UI")]
    [SerializeField] private GameObject clueUIPanel;  // The UI panel specific to this clue
    [SerializeField] private TextMeshProUGUI clueDescriptionText;  // Text to display the description
    [SerializeField] private Actions itemProducedAction; // This is the item produced
    

    // Show the clue's UI panel
    public void ShowClueUI()
    {
        if (clueUIPanel != null)
        {
            clueUIPanel.SetActive(true);
            clueDescriptionText.text = description;
        }
    }

    // Hide the clue's UI panel
    public void HideClueUI()
    {
        if (clueUIPanel != null)
        {
            clueUIPanel.SetActive(false);
            clueDescriptionText.text = ""; // Clear text when hiding
        }
    }

    #region Getters and Setters
    public int ClueId { get { return clueId; } set { clueId = value; } }
    public string Description { get { return description; } set { description = value; } }
    public Actions ItemProducedAction => itemProducedAction;
    #endregion
}
