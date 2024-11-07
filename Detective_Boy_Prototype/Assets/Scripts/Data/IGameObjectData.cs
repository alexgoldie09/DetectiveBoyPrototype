using UnityEngine;

public interface IGameObjectData
{
    int id { get; }
    Vector3 position { get; }
    bool isActive { get; }
}
