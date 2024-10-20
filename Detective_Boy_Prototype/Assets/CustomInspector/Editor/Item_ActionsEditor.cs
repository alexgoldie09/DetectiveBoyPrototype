using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Item_Actions))]
public class Item_ActionsEditor : Editor
{
    private Item_Actions source;
    SerializedProperty s_itemDatabase, s_canGiveItem, s_giveActions, s_doNotGiveActions, s_receiveActions, s_doNotReceiveActions, s_amount;

    private void OnEnable()
    {
        source = (Item_Actions)target;
        s_itemDatabase = serializedObject.FindProperty("itemDatabase");
        s_canGiveItem = serializedObject.FindProperty("canGiveItem");
        s_giveActions = serializedObject.FindProperty("giveActions");
        s_doNotGiveActions = serializedObject.FindProperty("doNotGiveActions");
        s_receiveActions = serializedObject.FindProperty("receiveActions");
        s_doNotReceiveActions = serializedObject.FindProperty("doNotReceiveActions");
        s_amount = serializedObject.FindProperty("amount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(s_itemDatabase, new GUIContent("Item Database: "));

        if(source.ItemDatabase != null)
        {
            // Draw popup or enum for selecting items
            source.ItemID = EditorGUILayout.Popup(source.ItemID, source.ItemDatabase.ItemNames.ToArray());

            EditorGUILayout.PropertyField(s_canGiveItem, new GUIContent("Give Item: "));

            // Draw item entry
            DrawItemEntry(source.CurrentItem);

            EditorExtensions.DrawActionsArray(s_giveActions, "Give Actions: ");

            EditorExtensions.DrawActionsArray(s_doNotGiveActions, "Do Not Give Actions: ");

            EditorExtensions.DrawActionsArray(s_receiveActions, "Receive Actions: ");

            EditorExtensions.DrawActionsArray(s_doNotReceiveActions, "Do Not Receive Actions: ");
        }

        if(GUI.changed)
        {
            if (source.ItemDatabase != null)
            {
                source.ChangeItem(source.ItemDatabase.GetItem(source.ItemID));
            }

            EditorUtility.SetDirty(source);
            EditorSceneManager.MarkSceneDirty(source.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawItemEntry(Item _item)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Item ID: " + _item.ItemId, GUILayout.Width(75f));
        EditorGUILayout.LabelField("Item Name: " + _item.ItemName);


        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        
        EditorGUILayout.LabelField("Item Description: " + _item.ItemDescription, GUILayout.Height(70f));

        if (_item.IsReward)
        {
            EditorGUILayout.LabelField("I am a reward!");
        }

        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();

        var spriteViewer = AssetPreview.GetAssetPreview(_item.ItemSprite);
        GUILayout.Label(spriteViewer);

        if (_item.AllowMultiple)
        {
            EditorGUILayout.PropertyField(s_amount);
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
