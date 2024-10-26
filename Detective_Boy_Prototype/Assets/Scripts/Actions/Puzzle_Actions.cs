using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Actions : Actions
{
    [Header("Puzzle Gameobject")]
    [SerializeField] private GameObject puzzlePrefab; // Reference to canvas UI

    public override void Act()
    {
        Extensions.isExamining = true;
        CreatePuzzle();
        StartCoroutine(OpenPuzzle());
    }

    private void CreatePuzzle()
    {
        if (PuzzleManager.instance.CurrentPuzzleCanvas == null)
        {
            // Instantiate the canvas only if it hasn't been instantiated
            PuzzleManager.instance.CurrentPuzzleCanvas = Instantiate(puzzlePrefab);
            PuzzleManager.instance.CurrentPuzzleCanvas.transform.SetParent(this.transform, false);
            PuzzleManager.instance.CurrentPuzzleCanvas.SetActive(true); // Ensure the canvas is active
            EnableCursor(true);
        }
    }

    private IEnumerator OpenPuzzle()
    {
        while (Extensions.isExamining)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                DestroyPuzzle();

                Extensions.isExamining = false;
            }
            yield return null;
        }
    }

    // Toggles the Canvas active state
    private void DestroyPuzzle()
    {
        bool isActive = PuzzleManager.instance.CurrentPuzzleCanvas.activeSelf;
        PuzzleManager.instance.CurrentPuzzleCanvas.SetActive(!isActive);

        if (!isActive)
        {
            EnableCursor(true);
        }
        else
        {
            EnableCursor(false);
            Destroy(PuzzleManager.instance.CurrentPuzzleCanvas);
            PuzzleManager.instance.CurrentPuzzleCanvas = null;
        }
    }

    // Manages cursor state based on canvas visibility
    private void EnableCursor(bool _enable)
    {
        Cursor.lockState = _enable ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = _enable;
    }
}
