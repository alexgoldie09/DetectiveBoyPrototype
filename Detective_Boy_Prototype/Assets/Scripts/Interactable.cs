using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private float interactionDistance = 2f; // Set the distance threshold
    [SerializeField] private GameObject displayUI; // Reference to the interactable text
    [SerializeField] private TextMeshProUGUI displayText; // Reference to the display text
    [SerializeField] private string defaultMessage = ""; // Reference to the message for text
    [SerializeField] private Actions[] actions; // Reference to interactable actions
    [SerializeField] private Camera mainCam; // Reference to main cam

    private void Start()
    {
        displayUI.SetActive(false);
        displayText.text = defaultMessage;

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if(mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    private void Update()
    {
        if (displayUI.activeSelf)
        {
            Interact();
        }
    }

    private void Interact()
    {
        displayUI.transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);

        if (Input.GetKeyDown(KeyCode.E) && CheckDistanceToPlayer() < interactionDistance)
        {
            displayUI.SetActive(false);
            StartCoroutine(ExecuteActionList());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player has entered.");
            displayUI.SetActive(true);
            displayText.text = defaultMessage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has left.");
            displayUI.SetActive(false);
        }
    }

    private IEnumerator ExecuteActionList()
    {
        for (int i = 0; i < actions.Length; i++)
        {
            yield return StartCoroutine(RunAction(i));
        }
    }

    private IEnumerator RunAction(int index)
    {
        yield return null;
        actions[index].Act();
    }

    // Function to check distance between interactable and player
    private float CheckDistanceToPlayer() => Vector3.Distance(transform.position, player.position);
}
