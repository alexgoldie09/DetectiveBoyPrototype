using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory; // Reference to player's Inventory ScriptableObject
    [SerializeField] private GameObject itemDisplayPrefab; // Prefab for displaying each item
    [SerializeField] private Transform contentPanel; // Parent panel for item displays
    [SerializeField] private TMP_Text descriptionText; // Text to display item descriptions

    private List<GameObject> itemDisplays = new List<GameObject>(); // List of instantiated item display objects

    //private void Start()
    //{
    //    if (inventory != null)
    //    {
    //        inventory.OnInventoryChanged += UpdateInventoryUI; // Subscribe to inventory changes
    //    }
    //}

    private void Start()
    {
        ClearItemDescription();
    }

    //private void OnDestroy()
    //{
    //    if (inventory != null)
    //    {
    //        inventory.OnInventoryChanged -= UpdateInventoryUI; // Unsubscribe to avoid memory leaks
    //    }
    //}

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventoryUI; // Subscribe to inventory changes
        }
        UpdateInventoryUI(); // Update when enabled
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateInventoryUI; // Unsubscribe to avoid memory leaks
        }
    }

    public void UpdateInventoryUI()
    {
        // Clear current UI
        foreach (GameObject display in itemDisplays)
        {
            Destroy(display);
        }
        itemDisplays.Clear();

        // Populate UI with current inventory items
        foreach (Item item in inventory.InventoryList)
        {
            GameObject itemDisplay = Instantiate(itemDisplayPrefab, contentPanel);
            itemDisplays.Add(itemDisplay);

            var itemImage = itemDisplay.transform.Find("ItemImage")?.GetComponent<Image>();
            var itemNameText = itemDisplay.transform.Find("ItemName")?.GetComponent<TMP_Text>();

            if (itemImage != null && item.ItemSprite != null) itemImage.sprite = item.ItemSprite;
            if (itemNameText != null) itemNameText.text = item.ItemName;

            // Add hover event listeners
            var eventTrigger = itemDisplay.AddComponent<EventTrigger>();

            // Pointer Enter
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { ShowItemDescription(item); });
            eventTrigger.triggers.Add(entryEnter);

            // Pointer Exit
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { ClearItemDescription(); });
            eventTrigger.triggers.Add(entryExit);
        }
    }

    // Display the item's description
    private void ShowItemDescription(Item item)
    {
        if (descriptionText != null)
        {
            descriptionText.text = item.ItemDescription;
        }
    }

    // Clear the description text when the pointer exits
    private void ClearItemDescription()
    {
        if (descriptionText != null)
        {
            descriptionText.text = "";
        }
    }

    //public void UpdateInventoryUI()
    //{
    //    if (contentPanel == null || itemDisplayPrefab == null || inventory == null)
    //    {
    //        Debug.LogError("InventoryUI is missing references in the Inspector.");
    //        return;
    //    }

    //    // Clear current UI
    //    foreach (GameObject display in itemDisplays)
    //    {
    //        Destroy(display);
    //    }
    //    itemDisplays.Clear();

    //    // Populate UI with current inventory items
    //    foreach (Item item in inventory.InventoryList)
    //    {
    //        if (item == null)
    //        {
    //            Debug.LogWarning("Encountered null item in inventory list.");
    //            continue;
    //        }

    //        GameObject itemDisplay = Instantiate(itemDisplayPrefab, contentPanel);
    //        itemDisplays.Add(itemDisplay);

    //        // Debug log for item properties
    //        Debug.Log($"Adding item to UI: ID={item.ItemId}, Name={item.ItemName}, Amount={item.Amount}, Sprite={item.ItemSprite}");

    //        // Set UI elements for each item
    //        var itemImage = itemDisplay.transform.Find("ItemImage")?.GetComponent<Image>();
    //        var itemNameText = itemDisplay.transform.Find("ItemName")?.GetComponent<TMP_Text>();
    //        //var itemQuantityText = itemDisplay.transform.Find("ItemQuantity")?.GetComponent<TMP_Text>();

    //        if (itemImage != null)
    //        {
    //            if (item.ItemSprite != null)
    //            {
    //                itemImage.sprite = item.ItemSprite;
    //            }
    //            else
    //            {
    //                Debug.LogWarning($"Item {item.ItemName} is missing a sprite.");
    //            }
    //        }
    //        if (itemNameText != null) itemNameText.text = item.ItemName;
    //        //if (itemQuantityText != null) itemQuantityText.text = "x" + item.Amount.ToString();
    //    }
    //}
}



