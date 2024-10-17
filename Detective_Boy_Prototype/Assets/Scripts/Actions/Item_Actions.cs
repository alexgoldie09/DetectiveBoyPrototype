using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Actions : Actions
{
    [Header("Variables")]
    [SerializeField] private ItemDatabase itemDatabase; // Reference to the item database
    [SerializeField] private bool canGiveItem; // Decides whether the item is being given or received
    [SerializeField] private int amount; // Reference to how many items to give
    [SerializeField] private Actions[] giveActions, doNotGiveActions, receiveActions, doNotReceiveActions; // Reference to choosing either side
    [SerializeField] private int itemId; // Reference to the item's ID
    [SerializeField] private Item currentItem; // Reference to the current

    public void ChangeItem(Item _item)
    {
        if (currentItem.ItemId == _item.ItemId)
        {
            return;
        }

        if (itemDatabase != null)
        {
            currentItem = Extensions.CopyItem(_item);
        }
    }

    public override void Act()
    {
        int itemAmountOwned = DataManager.instance.Inventory.CheckAmount(currentItem);

        // Check if give item is true, then give the item
        if (canGiveItem)
        {
            Debug.Log("I am giving item");
            // Check if own the item
            if (itemAmountOwned > 0)
            {
                // Check if the item can be multiples
                if(currentItem.AllowMultiple && amount <= itemAmountOwned)
                {
                    // Pass the item, and invoke actions
                    DataManager.instance.Inventory.ModifyItemAmount(currentItem, amount, true);
                    Extensions.RunActions(giveActions);
                    // Pass the suspect ID
                    NPCController npc = GetComponent<NPCController>();
                    if (npc != null && npc.IsSuspect)
                    {
                        DataManager.instance.SuspectFound(npc.SuspectId);
                    }
                }
                else if(!currentItem.AllowMultiple && itemAmountOwned == 1)
                {
                    // Remove the item from inventory, and invoke actions
                    DataManager.instance.Inventory.ModifyItemAmount(currentItem, itemAmountOwned, true);
                    Extensions.RunActions(giveActions);
                    // Pass the suspect ID
                    NPCController npc = GetComponent<NPCController>();
                    if (npc != null && npc.IsSuspect)
                    {
                        DataManager.instance.SuspectFound(npc.SuspectId);
                    }
                }
            }
            else
            {
                Debug.Log("You do not have the item.");
                // Do not have the item
                Extensions.RunActions(doNotGiveActions);
            }
        }
        // else receive item
        else
        {
            // Check if the item can be multiples
            if(currentItem.AllowMultiple)
            {
                DataManager.instance.Inventory.ModifyItemAmount(currentItem,amount);
                Extensions.RunActions(receiveActions);
            }
            // Else if we dont allow any multiples
            else if(!currentItem.AllowMultiple)
            {
                if(itemAmountOwned == 1)
                {
                    // Already have, invoke actions
                    Extensions.RunActions(doNotReceiveActions);
                }
                else
                {
                    // Add the item and invoke actions
                    DataManager.instance.Inventory.ModifyItemAmount(currentItem, 1);
                    Extensions.RunActions(receiveActions);
                }
            }
        }
    }

    #region Getters and Setters
    public int ItemID { get { return itemId; } set { itemId = value; } }
    public ItemDatabase ItemDatabase => itemDatabase;
    public Item CurrentItem => currentItem;
    #endregion
}
