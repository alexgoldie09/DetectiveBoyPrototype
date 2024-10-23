using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    //[SerializeField] private List<NPCData> suspectsList; // Reference to the suspects
    //private Dictionary<int, NPCController> npcDictionary = new Dictionary<int, NPCController>();
    //private Dictionary<int, NPCData> npcDictionary = new Dictionary<int, NPCData>();
    // Dictionary for storing all game object data (both NPCs and other objects)
    private Dictionary<int, IGameObjectData> gameObjectDataDict = new Dictionary<int, IGameObjectData>();
    private List<int> suspectIdNumbers = new List<int>(); // Reference to suspect ID numbers

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

    //public void SuspectRemoved(int _suspectId)
    //{
    //    // Find the npc with passed argument
    //    NPCController npcToRemove = suspectsList.Find(npc => npc.NPCId == _suspectId);

    //    // If the item exists, remove it
    //    if (npcToRemove != null)
    //    {
    //        suspectsList.Remove(npcToRemove);
    //        Debug.Log("Suspect: " + npcToRemove.gameObject.name + " has been removed.");
    //    }
    //}

    //public bool CanGiveReward(int _rewardId)
    //{
    //    if (npcDictionary.ContainsKey(_rewardId))
    //    {
    //        Debug.Log($"NPC with ID {_rewardId} exist in the dictionary.");
    //        // Check if any ID from npcIds exists in npcControllers
    //        foreach (int suspectId in suspectIdNumbers)
    //        {
    //            // Check if there is an NPCController with this ID
    //            bool exists = suspectsList.Any(npc => npc.NPCId == suspectId);

    //            if (exists)
    //            {
    //                Debug.Log($"NPC with ID {suspectId} exists in the list of NPCControllers.");
    //                return true;
    //            }
    //            else
    //            {
    //                Debug.LogWarning($"No NPC with ID {suspectId} found in the list of NPCControllers.");
    //                return false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("NPC IDs do not exist.");
    //        return false;
    //    }
    //    return false;
    //}

    #region Populate game data
    //// Function to add suspect data (only adds, no save logic)
    //public void AddSuspect(NPCData _npc)
    //{
    //    // Check if the NPC is not already in the list to avoid dupicates and is a suspect
    //    if (!suspectsList.Contains(_npc) && _npc.isSuspect)
    //    {
    //        suspectsList.Add(_npc);
    //        Debug.Log($"Added NPC with ID: {_npc.npcId} as suspect.");
    //    }
    //    else if (suspectsList.Contains(_npc))
    //    {
    //        Debug.LogWarning($"NPC with ID {_npc.npcId} already exists in the suspect list.");
    //    }
    //}

    // Function to add NPC data into the dictionary (only adds, no save logic)
    public void AddGameObjectToDictionary(int _id, IGameObjectData _gameObjectData)
    {
        if (!gameObjectDataDict.ContainsKey(_id))
        {
            gameObjectDataDict.Add(_id, _gameObjectData); // Add NPC data to dictionary if it doesn't already exist
            Debug.Log($"Added game object with ID: {_id}");
        }
    }

    //public void SuspectRevealed(int _suspectId)
    //{
    //    Debug.Log("You found suspect: " + _suspectId);
    //    foreach (NPCData suspect in suspectsList)
    //    {
    //        if (_suspectId == suspect.npcId)
    //        {
    //            suspectIdNumbers.Add(_suspectId);
    //            Debug.Log("Suspect #" + _suspectId + " has been found.");
    //        }
    //    }
    //}


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
    public List<int> SuspectIdNumbers => suspectIdNumbers;
    //public List<NPCController> SuspectsList => suspectsList;

    public void SetPrevSceneName(string _name) => PrevSceneName = _name;
    #endregion
}
