using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInventory_Actions : Actions
{
    [Header("Check Conditions")]
    [SerializeField] private ItemDatabase itemDatabase; // Reference to the item database
    [SerializeField] private int[] itemIdsToCheck; // Array of item IDs to check in inventory
    [SerializeField] private int[] requiredAmounts; // Corresponding required amounts for each item
    [SerializeField] private Actions[] actionsIfSufficient, actionsIfInsufficient; // Actions based on item availability

    public override void Act()
    {
        bool allItemsSufficient = true;

        for (int i = 0; i < itemIdsToCheck.Length; i++)
        {
            Item itemToCheck = itemDatabase.GetItem(itemIdsToCheck[i]);
            if (itemToCheck != null)
            {
                int itemAmountInInventory = DataManager.instance.Inventory.CheckAmount(itemToCheck);

                // Check if the required amount is met for this item
                if (itemAmountInInventory < requiredAmounts[i])
                {
                    allItemsSufficient = false;
                    break;
                }
            }
            else
            {
                Debug.LogWarning("Item with specified ID " + itemIdsToCheck[i] + " not found in the database.");
                allItemsSufficient = false;
                break;
            }
        }

        // Run actions based on whether all items have sufficient quantities
        if (allItemsSufficient)
        {
            Debug.Log("All required items found in sufficient quantity.");
            Extensions.RunActions(actionsIfSufficient);
        }
        else
        {
            Debug.Log("Insufficient quantity for one or more required items.");
            Extensions.RunActions(actionsIfInsufficient);
        }
    }
}
