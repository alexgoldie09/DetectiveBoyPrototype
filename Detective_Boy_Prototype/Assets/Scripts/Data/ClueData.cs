using UnityEngine;
using UnityEngine.UIElements;

public class ClueData : IGameObjectData
{
    public int id; // Unique ID for the game object
    public Vector3 position; // reference of gameobject position
    public string description; // description of gameobject
    public bool isActive; // Is this GameObject active

    // Implementing the interface properties
    int IGameObjectData.id => id;
    Vector3 IGameObjectData.position => position;
    bool IGameObjectData.isActive => isActive;

    public ClueData(int _id, string _description, Vector3 _position, bool _isActive)
    {
        this.id = _id;
        this.description = _description;
        this.position = _position;
        this.isActive = _isActive;
    }
}
