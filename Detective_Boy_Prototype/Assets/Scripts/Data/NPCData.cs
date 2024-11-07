using UnityEngine;

[System.Serializable]
public class NPCData : IGameObjectData
{
    public int id; // Unique ID for each NPC
    public Vector3 position; // Position in the game world
    public bool isSuspect; // Is this NPC a suspect
    public bool isActive; // Is this NPC GameObject active

    // Implementing the interface properties
    int IGameObjectData.id => id;
    Vector3 IGameObjectData.position => position;
    bool IGameObjectData.isActive => isActive;

    public NPCData(int _id, Vector3 _position, bool _isSuspect, bool _isActive)
    {
        this.id = _id;
        this.position = _position;
        this.isSuspect = _isSuspect;
        this.isActive = _isActive;
    }
}

