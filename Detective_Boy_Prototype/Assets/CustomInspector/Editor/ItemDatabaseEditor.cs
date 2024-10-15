using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    private ItemDatabase source;
    SerializedProperty s_items, s_itemNames;

    private void OnEnable()
    {
        source = (ItemDatabase)target;
        s_items = serializedObject.FindProperty("items");
        s_itemNames = serializedObject.FindProperty("itemNames");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //base.OnInspectorGUI();
        if(GUILayout.Button("Add Item"))
        {
            Item newItem = new Item(s_items.arraySize, "", "",null,false);
            source.AddItem(newItem);
        }

        for (int i = 0; i < s_items.arraySize; i++)
        {
            // draw the item entry
            DrawItemEntry(s_items.GetArrayElementAtIndex(i));
        }

        if(GUI.changed)
        {
            RecalculateID();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawItemEntry(SerializedProperty _item)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Item ID: " + _item.FindPropertyRelative("itemId").intValue, GUILayout.Width(75f));
        EditorGUILayout.PropertyField(_item.FindPropertyRelative("itemName"));

        if(GUILayout.Button("X", GUILayout.Width(20f)))
        {
            // delete the item
            s_itemNames.DeleteArrayElementAtIndex(_item.FindPropertyRelative("itemId").intValue);
            s_items.DeleteArrayElementAtIndex(_item.FindPropertyRelative("itemId").intValue);

            RecalculateID();
            return;
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_item.FindPropertyRelative("itemDescription"));

        GUILayout.BeginHorizontal();

        _item.FindPropertyRelative("itemSprite").objectReferenceValue = EditorGUILayout.ObjectField("Item Sprite: ", _item.FindPropertyRelative("itemSprite").objectReferenceValue, typeof(Sprite), false);
        EditorGUILayout.PropertyField(_item.FindPropertyRelative("allowMultiple"));

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void RecalculateID()
    {
        for (int i = 0; i < s_items.arraySize; i++)
        {
            s_items.GetArrayElementAtIndex(i).FindPropertyRelative("itemId").intValue = i;
            s_itemNames.GetArrayElementAtIndex(i).stringValue = s_items.GetArrayElementAtIndex(i).FindPropertyRelative("itemName").stringValue;
        }
    }
}
