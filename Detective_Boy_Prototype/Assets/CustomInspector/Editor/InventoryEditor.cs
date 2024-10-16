using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    private Inventory source;
    SerializedProperty s_inventoryList, s_itemDatabase;
    private int itemId;

    private void OnEnable()
    {
        source = (Inventory)target;

        s_inventoryList = serializedObject.FindProperty("inventoryList");
        s_itemDatabase = serializedObject.FindProperty("itemDatabase");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(s_itemDatabase);

        if(source.ItemDatabase != null)
        {
            itemId = EditorGUILayout.Popup(itemId, source.ItemDatabase.ItemNames.ToArray());

            if (GUILayout.Button("Add Item"))
            {
                Item newItem = Extensions.CopyItem(source.ItemDatabase.GetItem(itemId));
                source.AddItem(newItem);
            }

            for (int i = 0; i < s_inventoryList.arraySize; i++)
            {
                // draw the item entry
                DrawItemEntry(s_inventoryList.GetArrayElementAtIndex(i),i);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawItemEntry(SerializedProperty _item, int _id)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Item ID: " + _item.FindPropertyRelative("itemId").intValue, GUILayout.Width(75f));
        EditorGUILayout.LabelField("Item Name: "+ _item.FindPropertyRelative("itemName").stringValue);

        if (GUILayout.Button("X", GUILayout.Width(20f)))
        {
            // delete the item
            s_inventoryList.DeleteArrayElementAtIndex(_id);

            return;
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Item Description: " + _item.FindPropertyRelative("itemDescription").stringValue, GUILayout.Height(70f));

        GUILayout.BeginHorizontal();

        var spriteViewer = AssetPreview.GetAssetPreview(_item.FindPropertyRelative("itemSprite").objectReferenceValue);
        GUILayout.Label(spriteViewer);

        if (_item.FindPropertyRelative("allowMultiple").boolValue)
        {
            EditorGUILayout.PropertyField(_item.FindPropertyRelative("amount"));
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

}
