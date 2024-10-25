using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy the new one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Assign this as the instance
            instance = this;

            // Optionally, ensure this object persists across scenes
            DontDestroyOnLoad(gameObject);
        }

        LevelManager = GetComponentInChildren<LevelManager>();
    }

    [Header("Inventory")]
    [SerializeField] private Inventory inventory; // Reference to the inventory

    // Dictionary for storing all game object data (both NPCs and other objects)
    private Dictionary<int, IGameObjectData> gameObjectDataDict = new Dictionary<int, IGameObjectData>();

    // Dictionary to store quests
    public Dictionary<int, Quest> quests = new Dictionary<int, Quest>();

    public LevelManager LevelManager { get; private set; } // Reference to level manager
    public string PrevSceneName { get; private set; } // Reference to the previous scene

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Quest data

    public void RevealSuspect(int _questId)
    {
        if (quests.ContainsKey(_questId))
        {
            quests[_questId].SuspectRevealed = true;
        }
    }

    #endregion

    #region Gameobject data
    // Function to add NPC data into the dictionary (only adds, no save logic)
    public void AddGameObjectToDictionary(int _id, IGameObjectData _gameObjectData)
    {
        if (!gameObjectDataDict.ContainsKey(_id))
        {
            gameObjectDataDict.Add(_id, _gameObjectData); // Add NPC data to dictionary if it doesn't already exist
            Debug.Log($"Added game object with ID: {_id}");
        }
    }

    // Save an NPC or game object by ID
    public void SaveGameObject(int _id, IGameObjectData _gameObjectData)
    {
        gameObjectDataDict[_id] = _gameObjectData; // Save or update the data
    }

    // Load an NPC or game object by ID
    public IGameObjectData LoadGameObject(int _id)
    {
        if (gameObjectDataDict.TryGetValue(_id, out var _gameObjectData))
        {
            return _gameObjectData; // Return the data if it exists
        }
        return null; // Return null if not found
    }

    // Load all interactable states into a list
    public List<IGameObjectData> LoadAllInteractableStates()
    {
        return new List<IGameObjectData>(gameObjectDataDict.Values);
    }

    // Load all Clue states into a list
    public List<IGameObjectData> LoadAllClueStates()
    {
        return new List<IGameObjectData>(gameObjectDataDict.Values);
    }

    // Optional: Remove data from the dictionary if needed
    public void RemoveGameObject(int id)
    {
        if (gameObjectDataDict.ContainsKey(id))
        {
            gameObjectDataDict.Remove(id);
        }
    }
    #endregion

    #region Getters and Setters
    public Inventory Inventory => inventory;
    public Dictionary<int, IGameObjectData> GameObjectDataDict => gameObjectDataDict;

    public void SetPrevSceneName(string _name) => PrevSceneName = _name;
    #endregion
}
