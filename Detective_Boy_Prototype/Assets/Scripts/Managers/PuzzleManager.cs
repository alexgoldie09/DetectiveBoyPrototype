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

    [Header("Puzzle Actions")]
    [SerializeField] private Actions[] chainedActions; // Reference to chained actions
    private GameObject currentPuzzleCanvas; // Store the instantiated canvas

    private void Update()
    {
       
    }

    public void InitiateActions()
    {
        for(int i = 0; i < chainedActions.Length; i++)
        {
            chainedActions[i].Act();
        }
    }

    #region Getters and Setters
    public GameObject CurrentPuzzleCanvas { get { return currentPuzzleCanvas; } set { currentPuzzleCanvas = value;  } }
    #endregion
}
