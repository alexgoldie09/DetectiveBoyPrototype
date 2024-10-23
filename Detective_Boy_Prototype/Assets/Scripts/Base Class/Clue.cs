using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [Header("Clue Info")]
    [SerializeField] private int clueId; // Reference to Id number
    [TextArea]
    [SerializeField] private string description;  // This is the description that will be shown in the UI
    [SerializeField] private Actions itemProducedAction; // This is the item produced

    #region Getters and Setters
    public int ClueId { get { return clueId; } set { clueId = value; } }
    public string Description { get { return description; } set { description = value; } }
    public Actions ItemProducedAction => itemProducedAction;
    #endregion
}
