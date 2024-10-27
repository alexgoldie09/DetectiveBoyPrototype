using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if(receiveQuest)
            {
                questCanvas.SetActive(true);
            }
            else
            {
                questCanvas.SetActive(false);
            }
        }

    }

    public override void Act()
    {
        // Check if you are receiving a quest
        if (receiveQuest)
        {
            AddQuest();
            AddStepsToQuest();
            if(questCanvas != null)
            {
                Destroy(questCanvas);
            }
        }
        // Else you are completing a step
        else
        {
            CompleteStep();
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
            foreach (QuestStep step in questSteps)
            {
                // Check if the step already exists
                bool stepExists = DataManager.instance.quests[questId].Steps.Exists(s => s.stepId == step.stepId);

                if (!stepExists)
                {
                    // Step does not exist, so add it
                    DataManager.instance.quests[questId].Steps.Add(new QuestStep(step.stepId, step.description));
                    Debug.Log($"Added quest step #{step.stepId}: {step.description}");
                }
                else
                {
                    Debug.LogWarning($"Step {step.stepId} already exists in Quest {questId}.");
                }
        
            }
        }
    }

    public void CompleteStep()
    {
        if (DataManager.instance.quests.ContainsKey(questId))
        {
            var step = DataManager.instance.quests[questId].Steps.Find(s => s.stepId == stepId);
            if (step != null && !step.IsComplete)
            {
                step.IsComplete = true;
                Debug.Log($"Step {step.stepId} has been completed.");
            }
            else
            {
                Debug.Log("Step is finished.");
            }
        }
    }
}
