using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Animation_Actions : Actions
{
    [SerializeField] private List<AnimParameter> animList = new List<AnimParameter>(); // Reference to the animations which can be fired
    [SerializeField] private List<Actions> actionList = new List<Actions>(); // Reference to the list of actions that can be performed after

    private Animator anim; // Reference to objects animator

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        for(int i = 0; i < animList.Count; i++)
        {
            animList[i].InitHashID();
        }
    }

    public override void Act()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Extensions.isTalking = true;

        int i = 0;

        while(i < animList.Count)
        {
            yield return new WaitForSeconds(animList[i].InvokeDelay);

            anim.SetTrigger(animList[i].HashID);

            i++;

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(anim.GetNextAnimatorStateInfo(0).length);
        }

        Extensions.isTalking = false;

        for(int j = 0; j < actionList.Count; j++)
        {
            actionList[j].Act();
        }
    }

}

[System.Serializable]
public class AnimParameter
{
    [SerializeField] private string triggerName;
    [SerializeField] private float invokeDelay;

    public void InitHashID()
    {
        HashID = Animator.StringToHash(triggerName);
    }

    #region Getters and Setters
    public int HashID { get; private set; }
    public float InvokeDelay => invokeDelay;
    #endregion
}
