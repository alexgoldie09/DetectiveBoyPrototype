using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] protected int itemId; // Reference to the item's id number
    [SerializeField] protected string itemName; // Reference to the item's name
    [SerializeField] protected string itemDescription; // Reference to the item's description
    [SerializeField] protected Sprite itemSprite; // Reference to the item's sprite
    [SerializeField] protected bool allowMultiple; // Reference to whether we can have multiple items
    [SerializeField] protected int amount; // Reference to how many of these items we can have

    public Item(int _itemId, string _itemName, string _itemDescription, Sprite _itemSprite, bool _allowMultiple)
    {
        this.itemId = _itemId;
        this.itemName = _itemName;
        this.itemDescription = _itemDescription;
        this.itemSprite = _itemSprite;
        this.allowMultiple = _allowMultiple;
    }

    public void ModifyAmount(int _value) => amount += _value;

    #region Getters and Setters
    public int ItemId { get { return itemId; } set { itemId = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = value; } }
    public bool AllowMultiple { get { return allowMultiple; } set { allowMultiple = value; } }
    public int Amount { get { return amount; } set { amount = value; } }
    #endregion
}
