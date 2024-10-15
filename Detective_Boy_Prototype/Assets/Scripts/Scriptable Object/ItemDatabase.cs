using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemDatabase", menuName ="Custom Data/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<string> itemNames = new List<string>();

    public void AddItem(Item _item)
    {
        items.Add(_item);
        itemNames.Add(_item.ItemName);
    }

    public Item GetItem(int _id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemId == _id)
            {
                return items[i];
            }
        }

        return null;
    }

    #region Getters and Setters
    public List<string> ItemNames { get { return itemNames; } set { itemNames = value; } }
    #endregion
}
