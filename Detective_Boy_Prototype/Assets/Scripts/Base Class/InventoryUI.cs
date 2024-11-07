using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance; // Singleton instance

    [SerializeField] private Inventory inventory; // Reference to player's Inventory ScriptableObject
    [SerializeField] private GameObject itemDisplayPrefab; // Prefab for displaying each item
    [SerializeField] private Transform contentPanel; // Parent panel for item displays

    private List<GameObject> itemDisplays = new List<GameObject>(); // List of instantiated item display objects

    private void Awake()
    {
        // Set up singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventoryUI; // Subscribe to inventory changes
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateInventoryUI; // Unsubscribe to avoid memory leaks
        }
    }

    public void UpdateInventoryUI()
    {
        if (contentPanel == null || itemDisplayPrefab == null || inventory == null)
        {
            Debug.LogError("InventoryUI is missing references in the Inspector.");
            return;
        }

        // Clear current UI
        foreach (GameObject display in itemDisplays)
        {
            Destroy(display);
        }
        itemDisplays.Clear();

        // Populate UI with current inventory items
        foreach (Item item in inventory.InventoryList)
        {
            if (item == null)
            {
                Debug.LogWarning("Encountered null item in inventory list.");
                continue;
            }

            GameObject itemDisplay = Instantiate(itemDisplayPrefab, contentPanel);
            itemDisplays.Add(itemDisplay);

            // Debug log for item properties
            Debug.Log($"Adding item to UI: ID={item.ItemId}, Name={item.ItemName}, Amount={item.Amount}, Sprite={item.ItemSprite}");

            // Set UI elements for each item
            var itemImage = itemDisplay.transform.Find("ItemImage")?.GetComponent<Image>();
            var itemNameText = itemDisplay.transform.Find("ItemName")?.GetComponent<TMP_Text>();
            //var itemQuantityText = itemDisplay.transform.Find("ItemQuantity")?.GetComponent<TMP_Text>();

            if (itemImage != null)
            {
                if (item.ItemSprite != null)
                {
                    itemImage.sprite = item.ItemSprite;
                }
                else
                {
                    Debug.LogWarning($"Item {item.ItemName} is missing a sprite.");
                }
            }
            if (itemNameText != null) itemNameText.text = item.ItemName;
            //if (itemQuantityText != null) itemQuantityText.text = "x" + item.Amount.ToString();
        }
    }
}



