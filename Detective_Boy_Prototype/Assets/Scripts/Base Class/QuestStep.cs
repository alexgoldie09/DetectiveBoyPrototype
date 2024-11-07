using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep
{
    public int stepId;
    [TextArea]
    public string description;
    private bool isComplete;

    public QuestStep(int _stepId, string _description)
    {
        stepId = _stepId;
        description = _description;
        isComplete = false;
    }

    #region Getter and Setter
    public bool IsComplete { get { return isComplete; } set { isComplete = value; } }
    #endregion
}
