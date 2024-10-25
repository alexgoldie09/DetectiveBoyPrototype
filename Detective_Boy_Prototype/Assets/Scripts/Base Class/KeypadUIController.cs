using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadUIController : MonoBehaviour
{
    public string correctCode = "3579";   // The correct code
    private string enteredCode = "";      // The code the player enters
    public Actions[] chainedActions; // Reference to actions

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
            Unlock();
        }
        else
        {
            StartCoroutine(WaitForReset());
        }
    }

    private IEnumerator WaitForReset()
    {
        Debug.Log("Incorrect code. Resetting...");
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

    void Unlock()
    {
        Debug.Log("Keypad unlocked!");
        Extensions.isExamining = false;
        // Add any additional unlock logic or animations here
        Extensions.RunActions(chainedActions);
    }
}
