using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static Item CopyItem(Item _item)
    {
        Item newItem = new Item(_item.ItemId, _item.ItemName, _item.ItemDescription, _item.ItemSprite, _item.AllowMultiple);

        return newItem;
    }

    public static void RunActions(Actions[] _actions)
    {
        for(int i = 0; i < _actions.Length; i++)
        {
            _actions[i].Act();
        }
    }
}
