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
    }

    [Header("Data")]
    [SerializeField] private Inventory inventory; // Reference to the inventory
    [SerializeField] private List<NPCController> suspectsList; // Reference to the suspects
    private Dictionary<int, NPCController> npcDictionary = new Dictionary<int, NPCController>();
    private List<int> suspectIdNumbers = new List<int>(); // Reference to suspect ID numbers

    // Start is called before the first frame update
    void Start()
    {
        // Find all NPCController components in the scene
        NPCController[] allNPCs = FindObjectsOfType<NPCController>();

        // Populate the dictionary
        foreach (NPCController npc in allNPCs)
        {
            // Check if the NPCId is not already in the dictionary to avoid duplicate keys
            if (!npcDictionary.ContainsKey(npc.NPCId))
            {
                npcDictionary.Add(npc.NPCId, npc);
                Debug.Log($"Added NPC with ID: {npc.NPCId}");
            }
            else
            {
                Debug.LogWarning($"NPC with ID {npc.NPCId} already exists in the dictionary.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SuspectRevealed(int _suspectId)
    {
        Debug.Log("You found suspect: " + _suspectId);
        foreach (NPCController suspect in suspectsList)
        {
            if (_suspectId == suspect.NPCId)
            {
                suspectIdNumbers.Add(_suspectId);
                Debug.Log("Suspect #" + _suspectId+  " has been found.");
            }
        }
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

    public bool CanGiveReward(int _rewardId)
    {
        if (npcDictionary.ContainsKey(_rewardId))
        {
            Debug.Log($"NPC with ID {_rewardId} exist in the dictionary.");
            // Check if any ID from npcIds exists in npcControllers
            foreach (int suspectId in suspectIdNumbers)
            {
                // Check if there is an NPCController with this ID
                bool exists = suspectsList.Any(npc => npc.NPCId == suspectId);

                if (exists)
                {
                    Debug.Log($"NPC with ID {suspectId} exists in the list of NPCControllers.");
                    return true;
                }
                else
                {
                    Debug.LogWarning($"No NPC with ID {suspectId} found in the list of NPCControllers.");
                    return false;
                }
            }
        }
        else
        {
            Debug.LogWarning("NPC IDs do not exist.");
            return false;
        }
        return false;
    }

    #region Getters and Setters
    public Inventory Inventory => inventory;
    public List<NPCController> SuspectsList => suspectsList;
    #endregion
}
