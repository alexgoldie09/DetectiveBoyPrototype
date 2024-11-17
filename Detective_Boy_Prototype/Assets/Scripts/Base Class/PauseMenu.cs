using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI panels")]
    [SerializeField] private GameObject pauseMenuUI; // Main pause menu UI
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private InventoryUI inventoryUI; // Reference to InventoryUI component

    [Header("Tab buttons")]
    // References to each tab button
    [SerializeField] private Button inventoryTabButton;
    [SerializeField] private Button settingsTabButton;
    [SerializeField] private Button questTabButton;

    [Header("Quest Panel")]
    [SerializeField] private TMP_Text questDescriptionText; // Reference to display current quest step description
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private List<Quest> activeQuests; // List to hold active quests
    private int currentQuestIndex = 0; // Tracks the current quest being displayed

    private bool isPaused = false;

    private void Start()
    {
        // Ensure the pause menu is inactive at the start
        pauseMenuUI.SetActive(false);
        nextButton.onClick.AddListener(ShowNextQuest);
        previousButton.onClick.AddListener(ShowPreviousQuest);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !Extensions.isExamining && !Extensions.isTalking)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Extensions.isPaused = true;
        pauseMenuUI.SetActive(true);
        ShowInventory(); // Default to Inventory panel
        Time.timeScale = 0f; // Freeze game time

        // Unlock and show the mouse cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        Extensions.isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time

        // Hide and lock the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #region Show Panels
    // Show Inventory Panel and update Inventory UI
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        settingsPanel.SetActive(false);
        questPanel.SetActive(false);

        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI(); // Update InventoryUI on show
        }

        // Update button interactivity
        inventoryTabButton.interactable = false;
        settingsTabButton.interactable = true;
        questTabButton.interactable = true;
    }

    public void ShowSettings()
    {
        inventoryPanel.SetActive(false);
        settingsPanel.SetActive(true);
        questPanel.SetActive(false);

        settingsTabButton.interactable = false;
        inventoryTabButton.interactable = true;
        questTabButton.interactable = true;
    }

    public void ShowQuestPanel()
    {
        inventoryPanel.SetActive(false);
        settingsPanel.SetActive(false);
        questPanel.SetActive(true);

        // Fetch and display active quests on opening quest log
        activeQuests = GetActiveQuests();
        currentQuestIndex = 0;
        UpdateQuestLog(); // Update quest log on display

        questTabButton.interactable = false;
        inventoryTabButton.interactable = true;
        settingsTabButton.interactable = true;
    }
    #endregion

    #region Quest Log logic

    private List<Quest> GetActiveQuests()
    {
        // Retrieve all active quests
        List<Quest> activeQuests = new List<Quest>();
        foreach (var quest in DataManager.instance.quests.Values)
        {
            if (!quest.IsComplete)
            {
                activeQuests.Add(quest);
            }
        }
        return activeQuests;
    }

    private void UpdateQuestLog()
    {
        // If there are no active quests
        if (activeQuests == null || activeQuests.Count == 0)
        {
            questDescriptionText.text = "No active mysteries.";
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            return;
        }

        // If there is only one active quest, hide navigation buttons
        if (activeQuests.Count == 1)
        {
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
        }
        else
        {
            // Enable navigation buttons if there are multiple active quests
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);

            // Update button interactivity based on the quest index
            nextButton.interactable = currentQuestIndex < activeQuests.Count - 1;
            previousButton.interactable = currentQuestIndex > 0;
        }

        // Display the current quest and step
        Quest currentQuest = activeQuests[currentQuestIndex];
        QuestStep currentStep = currentQuest.Steps.Find(step => !step.IsComplete);

        if (currentStep != null)
        {
            questDescriptionText.text = $"Mystery #{currentQuest.QuestID}: {currentStep.description}";
        }
    }

    private void ShowNextQuest()
    {
        if (currentQuestIndex < activeQuests.Count - 1)
        {
            currentQuestIndex++;
            UpdateQuestLog();
        }
    }

    private void ShowPreviousQuest()
    {
        if (currentQuestIndex > 0)
        {
            currentQuestIndex--;
            UpdateQuestLog();
        }
    }
#endregion

    public void QuitGame()
    {
        Debug.Log("You are quitting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
