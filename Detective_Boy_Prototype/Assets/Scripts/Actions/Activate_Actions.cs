using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Actions : Actions
{
    [SerializeField] private List<CustomGameObject> customGameObjects = new List<CustomGameObject>(); // Reference to the list of custom game objects

    public override void Act()
    {
        for(int i = 0; i < customGameObjects.Count; i++)
        {
            customGameObjects[i].CustomGO.SetActive(customGameObjects[i].ActiveStatus);
        }
    }
}

[System.Serializable]
public class CustomGameObject
{
    [SerializeField] private GameObject customGO; // Reference to the game object
    [SerializeField] private bool activeStatus; // Reference to whether the object is active

    #region Getters and Setters
    public GameObject CustomGO => customGO;
    public bool ActiveStatus => activeStatus;
    #endregion
}
