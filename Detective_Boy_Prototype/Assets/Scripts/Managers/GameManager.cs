using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

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
        }
    }

    [Header("Gameobjects")]
    public Interactable[] allInteractables; // Array of all interactables in the scene
    public Clue[] allClues; // Array of all clues in the scene

    [Header("Spawner")]
    [SerializeField] private List<Spawner> spawnEntries = new List<Spawner>();
    private GameObject player; // Reference to the player

    private void Start()
    {
        // Find all NPCs in the scene when the scene starts
        allInteractables = FindObjectsOfType<Interactable>(true);

        // Find all Clues in the scene when the scene starts
        allClues = FindObjectsOfType<Clue>(true);

        // Find player
        player = GameObject.FindGameObjectWithTag("Player");

        // Add to dictionary (if required)
        PopulateDataManager();

        // Load their states after finding them
        LoadAllInteractableStates();
        LoadAllClueStates();

        // Reposition player on spawn
        Reposition();
    }

    //public void SuspectRevealed(int _suspectId)
    //{
    //    Debug.Log("You found suspect: " + _suspectId);

    //    if (DataManager.instance.NPCDictionary.ContainsKey(_suspectId))
    //    {
    //        DataManager.instance.SuspectIdNumbers.Add(_suspectId);
    //        Debug.Log("Suspect #" + _suspectId + " has been found.");
    //    }
    //}

    private void Reposition()
    {
        for(int i = 0; i < spawnEntries.Count; i++)
        {
            if(DataManager.instance.PrevSceneName == spawnEntries[i].PrevSceneName)
            {
                player.transform.position = spawnEntries[i].SpawnPos;
                player.transform.rotation = Quaternion.LookRotation(spawnEntries[i].SpawnDir);
            }
        }
    }

    private void PopulateDataManager()
    {
        // Populate NPC data
        foreach (var interactable in allInteractables)
        {
            InteractableData interactableData = new InteractableData(interactable.Id, interactable.transform.position, interactable.gameObject.activeSelf);

            // Add to DataManager
            DataManager.instance.AddGameObjectToDictionary(interactable.Id, interactableData);
        }

        // Populate clue data
        foreach (var clue in allClues)
        {
            ClueData clueData = new ClueData(clue.ClueId, clue.Description, clue.transform.position, clue.gameObject.activeSelf);

            // Add to DataManager
            DataManager.instance.AddGameObjectToDictionary(clue.ClueId, clueData);
        }
    }

    #region Interactable Data Handling
    public void SaveInteractables(int _id)
    {
        Interactable interactable = FindInteractableById(_id);
        if (interactable != null)
        {
            InteractableData interactableData = new InteractableData(interactable.Id, interactable.transform.position, interactable.gameObject.activeSelf);

            DataManager.instance.SaveGameObject(_id, interactableData);
        }
    }

    public void SaveAllInteractableStates()
    {
        foreach (var interactable in allInteractables)
        {
            SaveInteractables(interactable.Id); // Save each NPC's state
            Debug.Log($"Saved game object with ID: {interactable.Id}.");
        }
    }

    public void LoadInteractables(int _id)
    {
        var interactableData = DataManager.instance.LoadGameObject(_id) as InteractableData;
        if (interactableData != null)
        {
            // Use interactableData to restore interactable state
            // For example, you can set the NPC's position, health, etc.
            var interactable = FindInteractableById(_id); // Implement this method to find the NPC in the scene
            if (interactable != null)
            {
                interactable.transform.position = interactableData.position;
                interactable.gameObject.SetActive(interactableData.isActive);
                Debug.Log($"Loaded interactable with ID: {interactableData.id}.");
            }
        }
    }

    public void LoadAllInteractableStates()
    {
        var allInteractableData = DataManager.instance.LoadAllInteractableStates();
        foreach (var data in allInteractableData)
        {
            if (data is InteractableData interactableData)
            {
                LoadInteractables(interactableData.id); // Restore state for each NPC
            }
        }
    }

    private Interactable FindInteractableById(int _id)
    {
        // Implement a method to find the interactable by its ID in the current scene
        foreach (var interactable in allInteractables)
        {
            if (interactable.Id == _id)
            {
                return interactable;
            }
        }
        return null; // Return null if not found
    }
    #endregion

    #region Clue Data Handling
    public void SaveClue(int _id)
    {
        Clue clue = FindClueById(_id);
        if (clue != null)
        {
            ClueData clueData = new ClueData(clue.ClueId, clue.Description, clue.transform.position, clue.gameObject.activeSelf);

            DataManager.instance.SaveGameObject(_id, clueData);
        }
    }

    public void SaveAllClueStates()
    {
        foreach (var clue in allClues)
        {
            SaveClue(clue.ClueId); // Save each NPC's state
            Debug.Log($"Saved Clue with ID: {clue.ClueId}.");
        }
    }

    public void LoadClue(int _id)
    {
        var clueData = DataManager.instance.LoadGameObject(_id) as ClueData;
        if (clueData != null)
        {
            // Use clueData to restore Clue state
            // For example, you can set the clue's position, active status, etc.
            var clue = FindClueById(_id); // Implement this method to find the NPC in the scene
            if (clue != null)
            {
                clue.Description = clueData.description;
                clue.gameObject.SetActive(clueData.isActive);
                Debug.Log($"Loaded Clue with ID: {clueData.id}.");
            }
        }
    }

    public void LoadAllClueStates()
    {
        var allClueData = DataManager.instance.LoadAllClueStates();
        foreach (var data in allClueData)
        {
            if (data is ClueData clueData)
            {
                LoadClue(clueData.id); // Restore state for each NPC
            }
        }
    }

    private Clue FindClueById(int _id)
    {
        // Implement a method to find the Clue by its ID in the current scene
        foreach (var clue in allClues)
        {
            if (clue.ClueId == _id)
            {
                return clue;
            }
        }
        return null; // Return null if not found
    }

    #endregion
}

[System.Serializable]
public class Spawner
{
    [SerializeField] private string prevSceneName;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 spawnDir;

    #region Getters and Setters
    public string PrevSceneName => prevSceneName;
    public Vector3 SpawnPos => spawnPos;
    public Vector3 SpawnDir => spawnDir;
    #endregion
}
