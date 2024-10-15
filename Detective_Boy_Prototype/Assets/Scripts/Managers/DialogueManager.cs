using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

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

            // Optionally, ensure this object persists across scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI messageText; // Reference to the message texts
    [SerializeField] private TextMeshProUGUI interrogateText, accuseText; // Reference to the button text
    [SerializeField] private GameObject dialoguePanel; // Reference to the dialogue canvas
    [SerializeField] private GameObject buttonContainer; // Reference to the button holder
    [SerializeField] private Button interrogateButton, accuseButton; // Reference to the actions button

    private List<string> currentMessages = new List<string>();
    private int msgId = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Set dialogue panel to deactivate on start
        dialoguePanel.SetActive(false);
    }

    public void ShowMessages(List<string> _currentMessages,bool _enableDialog, List<Actions> _interrogateActions = null,
        List<Actions> _accuseActions = null, string _interrogateMessage = "Interrogate", string _accuseMessage = "Accuse")
    {
        // Reset message ID to zero
        msgId = 0;
        // Remove buttons at beginning
        buttonContainer.SetActive(false);
        // Set list to be the passed list
        currentMessages = _currentMessages;
        // Show dialogue panel
        dialoguePanel.SetActive(true);
        // Check if enable dialogue is true
        if(_enableDialog)
        {
            /* Set text to the appropriate message,
             * set buttons to remove all listeners, 
             * then add new listener using function, and finally
             * set dialogue panel to be inactive */
            interrogateText.text = _interrogateMessage;
            interrogateButton.onClick.RemoveAllListeners();
            interrogateButton.onClick.AddListener(delegate
            {
                dialoguePanel.SetActive(false);

                if (_interrogateActions != null)
                {
                    AssignActionsToButton(_interrogateActions);
                }
            });

            accuseText.text = _accuseMessage;
            accuseButton.onClick.RemoveAllListeners();
            accuseButton.onClick.AddListener(delegate
            {
                dialoguePanel.SetActive(false);

                if (_accuseActions != null)
                {
                    AssignActionsToButton(_accuseActions);
                }
            });

        }
        // Start coroutine to display all text
        StartCoroutine(ShowMultipleMessages(_enableDialog));
    }

    private IEnumerator ShowMultipleMessages(bool _enableDialog)
    {
        // Display first message
        messageText.text = currentMessages[msgId];
        // Enable cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        // Enter while loop until all message have been displayed
        while (msgId < currentMessages.Count)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                // Increment to next message
                msgId++;
                // Check if message Id is still less than total count
                if (msgId < currentMessages.Count)
                {
                    // Display message
                    messageText.text = currentMessages[msgId];
                }
                // Check if message Id is equal to the count minus one and dialogue choices enabled
                if (_enableDialog && msgId == currentMessages.Count - 1)
                {
                    // Activate buttons if it is
                    buttonContainer.SetActive(true);
                }
            }
            yield return null;
        }
        // Check if dialogue choices are enabled
        if (!_enableDialog)
        {
            // Set panel to inactive
            dialoguePanel.SetActive(false);
            // Disable cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Allow player to move again
            PlayerController.instance.IsTalking = false;
        }
    }

    public void AssignActionsToButton(List<Actions> _actions)
    {
        List<Actions> localActions = _actions;

        for(int i = 0; i < localActions.Count; i++)
        {
            localActions[i].Act();
        }
    }
}
