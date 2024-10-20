using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Animation_Actions))]
public class Animation_ActionsEditor : Editor
{
    private Animation_Actions source;
    SerializedProperty s_animList, s_actionList;

    private void OnEnable()
    {
        source = (Animation_Actions)target;
        s_animList = serializedObject.FindProperty("animList");
        s_actionList = serializedObject.FindProperty("actionList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Add Animation Parameter"))
        {
            s_animList.InsertArrayElementAtIndex(s_animList.arraySize);
        }

        // Draw anim inspector
        for(int i = 0; i < s_animList.arraySize; i++)
        {
            DrawAnimInspector(s_animList.GetArrayElementAtIndex(i), i);
        }

        EditorExtensions.DrawActionsArray(s_actionList, "Chained Actions: ");
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawAnimInspector(SerializedProperty _anim, int _id)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(_anim.FindPropertyRelative("triggerName"), new GUIContent("Trigger Name: "));

        if (GUILayout.Button("x", GUILayout.Width(20f)))
        {
            s_animList.DeleteArrayElementAtIndex(_id);
            return;
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_anim.FindPropertyRelative("invokeDelay"), new GUIContent("Delay (in sec): "));

        GUILayout.EndVertical();
    }
}
