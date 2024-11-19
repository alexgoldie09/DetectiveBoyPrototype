using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Quest
{
    public int QuestID { get; private set; }
    public bool IsComplete { get; set; }
    private bool suspectRevealed;
    public List<QuestStep> Steps { get; private set; }

    public Quest(int _questId)
    {
        QuestID = _questId;
        IsComplete = false;
        suspectRevealed = false;
        Steps = new List<QuestStep>();
    }

    #region Getters and Setters
    public bool SuspectRevealed { get { return suspectRevealed; } set { suspectRevealed = value; } }

    public bool IsCompleted
    {
        get
        {
            // Check if all steps in the quest are marked as complete
            return Steps.All(step => step.IsComplete);
        }
    }
    #endregion

}
