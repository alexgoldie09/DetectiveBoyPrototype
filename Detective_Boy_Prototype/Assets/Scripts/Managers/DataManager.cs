using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int suspectCount; // Reference to how many suspects have been found

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SuspectFound(int _suspectId)
    {
        Debug.Log("You found suspect: " + _suspectId);
        foreach (NPCController suspect in suspectsList)
        {
            if (_suspectId == suspect.SuspectId)
            {
                Debug.Log("Suspect #" + suspectCount +  " gone.");
                suspectCount++;
                suspect.gameObject.SetActive(false);
            }
        }
    }

    #region Getters and Setters
    public Inventory Inventory => inventory;
    public List<NPCController> SuspectsList => suspectsList;
    #endregion
}
