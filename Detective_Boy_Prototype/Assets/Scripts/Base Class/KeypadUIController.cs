using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadUIController : MonoBehaviour
{
    public string correctCode = "3579";   // The correct code
    private string enteredCode = "";      // The code the player enters


    public TextMeshProUGUI displayText;              // Reference to the Text UI displaying the code

    // Called by each button when pressed
    public void EnterDigit(string digit)
    {
        if (enteredCode.Length < correctCode.Length)
        {
            enteredCode += digit;
            UpdateDisplay();
        }

        // If the code is complete, check it
        if (enteredCode.Length == correctCode.Length)
        {
            CheckCode();
        }
    }

    // Method to check if the code is correct
    void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            StartCoroutine(Unlock());
        }
        else
        {
            StartCoroutine(WaitForReset());
        }
    }

    private IEnumerator WaitForReset()
    {
        Debug.Log("Incorrect code. Resetting...");
        enteredCode = "Try again!";
        UpdateDisplay();
        yield return new WaitForSeconds(1f);
        enteredCode = "";
        UpdateDisplay();
    }

    // Update the display text with the current entered code
    void UpdateDisplay()
    {
        displayText.text = enteredCode;
    }

    // Clear the entered code
    public void ClearCode()
    {
        enteredCode = "";
        UpdateDisplay();
    }

    private IEnumerator Unlock()
    {
        Debug.Log("Keypad unlocked!");
        enteredCode = "Unlocked!";
        UpdateDisplay();
        yield return new WaitForSeconds(1f);
        Extensions.isExamining = false;
        PuzzleManager.instance.InitiateActions();
        EnableCursor(false);
        PuzzleManager.instance.CurrentPuzzleCanvas = null;
    }

    // Manages cursor state based on canvas visibility
    private void EnableCursor(bool _enable)
    {
        Cursor.lockState = _enable ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = _enable;
    }
}
