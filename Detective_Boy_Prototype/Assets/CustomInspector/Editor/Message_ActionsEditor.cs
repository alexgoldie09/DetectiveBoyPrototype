using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Message_Actions))]
public class Message_ActionsEditor : Editor
{
    private Message_Actions source;
    SerializedProperty s_messages, s_enableDialog, s_interrogateMessage, s_accuseMessage, s_interrogateActions, s_accuseActions;

    private void OnEnable()
    {
        source = (Message_Actions)target;
        s_messages = serializedObject.FindProperty("messages");
        s_enableDialog = serializedObject.FindProperty("enableDialog");
        s_interrogateMessage = serializedObject.FindProperty("interrogateMessage");
        s_accuseMessage = serializedObject.FindProperty("accuseMessage");
        s_interrogateActions = serializedObject.FindProperty("interrogateActions");
        s_accuseActions = serializedObject.FindProperty("accuseActions");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Show add message
        if (GUILayout.Button("Add Message"))
        {
            s_messages.InsertArrayElementAtIndex(s_messages.arraySize);
        }
        // Loop through message list
        for(int i = 0; i < s_messages.arraySize; i++)
        {
            DrawMessageEntry(s_messages.GetArrayElementAtIndex(i), "Message " + (i+1), i);
        }

        // Show enableDialog toggle, if enabled, show dialog properties
        GUILayout.BeginVertical("box");

        EditorGUILayout.PropertyField(s_enableDialog, new GUIContent("Enable Dialog: "));

        if(s_enableDialog.boolValue)
        {
            EditorGUILayout.PropertyField(s_interrogateMessage, new GUIContent("Interrogate Button Label: "), GUILayout.Height(30f));
            EditorExtensions.DrawActionsArray(s_interrogateActions, "Interrogate Actions");
            EditorGUILayout.PropertyField(s_accuseMessage, new GUIContent("Accuse Button Label: "), GUILayout.Height(30f));
            EditorExtensions.DrawActionsArray(s_accuseActions, "Accuse Actions");
        }

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMessageEntry(SerializedProperty _messageEntry, string _label, int _id)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(_messageEntry, new GUIContent(_label), GUILayout.Height(50f));

        if(GUILayout.Button("x", GUILayout.Width(20f)))
        {
            s_messages.DeleteArrayElementAtIndex(_id);
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
