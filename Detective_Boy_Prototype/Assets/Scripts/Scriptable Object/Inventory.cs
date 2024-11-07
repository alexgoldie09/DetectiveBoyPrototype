using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Custom Data/Inventory Data")]
public class Inventory : ScriptableObject
{
    [SerializeField] private ItemDatabase itemDatabase; // Reference to the item database
    [SerializeField] List<Item> inventoryList = new List<Item>(); // Reference to the inventory
    public Action OnInventoryChanged; // Event triggered on inventory change

    public void AddItem(Item _item)
    {
        inventoryList.Add(_item);
        OnInventoryChanged?.Invoke(); // Trigger update
    }

    public void ModifyItemAmount(Item _item, int _amount = 1, bool _giveItem = false)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].ItemId == _item.ItemId)
            {
                if (inventoryList[i].AllowMultiple)
                {
                    inventoryList[i].ModifyAmount(_giveItem ? -_amount : _amount);

                    if (inventoryList[i].Amount <= 0 && _giveItem)
                    {
                        inventoryList.RemoveAt(i);
                        OnInventoryChanged?.Invoke(); // Trigger update after item removal
                    }
                    else
                    {
                        OnInventoryChanged?.Invoke(); // Trigger update after modifying item amount
                    }
                }
                else
                {
                    inventoryList.RemoveAt(i);
                    OnInventoryChanged?.Invoke(); // Trigger update after removing non-stackable item
                }

                return;
            }
        }

        Item newItem = Extensions.CopyItem(_item);
        newItem.ModifyAmount(_amount);

        AddItem(newItem);
        OnInventoryChanged?.Invoke(); // Trigger update after adding new item
    }

    public int CheckAmount(Item _item)
    {
        for(int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].ItemId == _item.ItemId)
            {
                if (inventoryList[i].AllowMultiple)
                {
                    return inventoryList[i].Amount;
                }
                else
                {
                    return 1;
                }
            }
        }

        return 0;
    }

    #region Getter and Setter
    public ItemDatabase ItemDatabase { get { return itemDatabase; } set { itemDatabase = value; } }
    public List<Item> InventoryList => inventoryList;
    #endregion
}
