using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Actions : Actions
{
    [SerializeField] private List<CustomGameObject> customGameObjects = new List<CustomGameObject>(); // Reference to the list of custom game objects

    public override void Act()
    {
        StartCoroutine(ActivateGameobject());
    }

    private IEnumerator ActivateGameobject()
    {
        for (int i = 0; i < customGameObjects.Count; i++)
        {
            yield return new WaitForSeconds(customGameObjects[i].ActivateTime);
            customGameObjects[i].CustomGO.SetActive(customGameObjects[i].ActiveStatus);
            //NPCController npc = GetComponent<NPCController>();
            //if (npc != null && npc.IsSuspect)
            //{
            //    DataManager.instance.SuspectRemoved(npc.NPCId);
            //}
        }
    }
}

[System.Serializable]
public class CustomGameObject
{
    [SerializeField] private GameObject customGO; // Reference to the game object
    [SerializeField] private bool activeStatus; // Reference to whether the object is active
    [SerializeField] private float activateTime; // Reference to if there is a delay before activation/deactivation
    #region Getters and Setters
    public GameObject CustomGO => customGO;
    public bool ActiveStatus => activeStatus;
    public float ActivateTime => activateTime;
    #endregion
}
