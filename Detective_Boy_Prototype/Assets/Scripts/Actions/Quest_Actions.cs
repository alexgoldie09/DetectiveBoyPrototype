using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Quest_Actions : Actions
{
    [Header("Adding Quest")]
    [SerializeField] private int questId; // Reference to quest ID
    [SerializeField] private string questTitle; // Reference to quest title
    [SerializeField] private List<QuestStep> questSteps = new List<QuestStep>(); // Reference to the quest steps
    [SerializeField] private bool receiveQuest; // Reference to whether you are receiving a quest
    [SerializeField] private GameObject questCanvas; // Reference to the quest icon
    [Header("Completing Step")]
    [SerializeField] private List<int> stepIds = new List<int>(); // List of steps to complete

    private void Start()
    {
        if (questCanvas != null)
        {
            // Check if the quest exists in the DataManager
            if (DataManager.instance.quests.ContainsKey(questId))
            {
                // If the quest exists, hide the canvas
                questCanvas.SetActive(false);
                Debug.Log($"Quest {questId} already exists. Hiding quest icon.");
            }
            else
            {
                // If the quest does not exist, show the canvas if set to receiveQuest
                questCanvas.SetActive(receiveQuest);
                Debug.Log($"Quest {questId} does not exist. Icon visibility set to {receiveQuest}.");
            }
        }
    }

    public override void Act()
    {
        // Check if the player is supposed to receive a quest
        if (receiveQuest)
        {
            // Prevent adding the quest if it's already complete
            if (DataManager.instance.quests.ContainsKey(questId))
            {
                if (DataManager.instance.quests[questId].IsComplete)
                {
                    Debug.Log($"Quest #{questId} is already complete. Cannot receive it again.");
                    return; // Exit the function early
                }
            }

            // Add the quest if it doesn't already exist or is incomplete
            AddQuest();
            AddStepsToQuest();

            // Remove the quest icon if it exists
            if (questCanvas != null)
            {
                Destroy(questCanvas);
            }
        }
        else
        {
            // Completing quest steps
            CompleteSteps();
            Debug.Log($"Checked steps for quest {questId}. Quest completion status: {DataManager.instance.quests[questId].IsComplete}");

        }
    }

    public void AddQuest()
    {
        if (!DataManager.instance.quests.ContainsKey(questId))
        {
            DataManager.instance.quests[questId] = new Quest(questId, questTitle);
            Debug.Log($"Added quest #{questId}");
        }
    }

    public void AddStepsToQuest()
    {
        if (DataManager.instance.quests.ContainsKey(questId))
        {
            var quest = DataManager.instance.quests[questId];

            foreach (QuestStep step in questSteps)
            {
                // Check if the step already exists
                bool stepExists = quest.Steps.Exists(s => s.stepId == step.stepId);

                if (!stepExists)
                {
                    // Step does not exist, so add it
                    quest.Steps.Add(new QuestStep(step.stepId, step.description));
                    Debug.Log($"Added quest step #{step.stepId}: {step.description}");
                }
                else
                {
                    Debug.LogWarning($"Step {step.stepId} already exists in Quest {questId}.");
                }
        
            }

            // Update IsComplete status in case all steps are already complete
            quest.IsComplete = quest.Steps.All(s => s.IsComplete);
            Debug.Log($"Quest {questId} IsComplete updated: {quest.IsComplete}");
        }
    }

    public void CompleteSteps()
    {
        if (DataManager.instance.quests.ContainsKey(questId))
        {
            var quest = DataManager.instance.quests[questId];

            foreach (int id in stepIds)
            {
                var step = quest.Steps.Find(s => s.stepId == id);

                if (step != null && !step.IsComplete)
                {
                    step.IsComplete = true;
                    Debug.Log($"Step {step.stepId} has been completed.");
                }
                else
                {
                    Debug.LogWarning($"Step {id} is already complete or does not exist.");
                }
            }

            // Check if all steps are complete and update the quest's status
            if (quest.Steps.All(s => s.IsComplete))
            {
                quest.IsComplete = true;
                Debug.Log($"Quest {questId} is now complete!");
            }
        }
        else
        {
            Debug.LogWarning($"Quest {questId} does not exist.");
        }
    }
}
