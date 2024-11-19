using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Quest_Actions : Actions
{
    [Header("Adding Quest")]
    [SerializeField] private int questId; // Reference to quest ID
    [SerializeField] private List<QuestStep> questSteps = new List<QuestStep>(); // Reference to the quest steps
    [SerializeField] private bool receiveQuest; // Reference to whether you are receiving a quest
    [SerializeField] private GameObject questCanvas; // Reference to the quest icon
    [Header("Completing Step")]
    [SerializeField] private int stepId; // Reference to step to complete

    private void Start()
    {
        if (questCanvas != null)
        {
            // Check if the quest exists and is incomplete
            if (DataManager.instance.quests.ContainsKey(questId))
            {
                // If the quest is complete, hide the icon
                if (DataManager.instance.quests[questId].IsComplete)
                {
                    questCanvas.SetActive(false);
                }
                else
                {
                    questCanvas.SetActive(receiveQuest);
                }
            }
            else
            {
                // Quest does not exist, show the icon if set to receiveQuest
                questCanvas.SetActive(receiveQuest);
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
            // Completing a quest step
            CompleteStep();
            Debug.Log($"Quest {questId} is complete: {DataManager.instance.quests[questId].IsComplete}");

        }
    }

    public void AddQuest()
    {
        if (!DataManager.instance.quests.ContainsKey(questId))
        {
            DataManager.instance.quests[questId] = new Quest(questId);
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

    public void CompleteStep()
    {
        if (DataManager.instance.quests.ContainsKey(questId))
        {
            var quest = DataManager.instance.quests[questId];
            var step = DataManager.instance.quests[questId].Steps.Find(s => s.stepId == stepId);
            if (step != null && !step.IsComplete)
            {
                step.IsComplete = true;
                Debug.Log($"Step {step.stepId} has been completed.");
                // Check if all steps are complete and update the IsComplete property
                if (quest.Steps.All(s => s.IsComplete))
                {
                    quest.IsComplete = true;
                    Debug.Log($"Quest {questId} is now complete!");
                }
            }
            else
            {
                Debug.Log("Step is finished or does not exist.");
            }
        }
    }
}
