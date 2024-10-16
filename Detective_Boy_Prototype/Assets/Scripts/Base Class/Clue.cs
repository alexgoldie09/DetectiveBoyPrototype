using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [Header("Clue Info")]
    [TextArea]
    [SerializeField] private string description;  // This is the description that will be shown in the UI
    [SerializeField] private Actions itemProducedAction; // This is the item produced

    #region Getters and Setters
    public string Description => description;
    public Actions ItemProducedAction => itemProducedAction;
    #endregion
}
