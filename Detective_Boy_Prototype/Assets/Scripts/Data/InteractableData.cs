using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableData : IGameObjectData
{
    public int id; // Unique ID for each NPC
    public Vector3 position; // Position in the game world
    public bool isActive; // Is this GameObject active

    // Implementing the interface properties
    int IGameObjectData.id => id;
    Vector3 IGameObjectData.position => position;
    bool IGameObjectData.isActive => isActive;

    public InteractableData(int _id, Vector3 _position, bool _isActive)
    {
        this.id = _id;
        this.position = _position;
        this.isActive = _isActive;
    }
}
