using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message_Actions : Actions
{
    [Header("Variables")]
    [Multiline(5)]
    [SerializeField] private List<string> messages; // Reference to the message
    [SerializeField] private bool enableDialog; // Reference to enable a dialogue option
    [SerializeField] private string interrogateMessage, accuseMessage; // Reference to message for picking dialogue
    [SerializeField] List<Actions> interrogateActions, accuseActions; // Reference to choosing either side

    public override void Act()
    {
        DialogueManager.instance.ShowMessages(messages, enableDialog, interrogateActions, accuseActions, interrogateMessage, accuseMessage);
    }
}
