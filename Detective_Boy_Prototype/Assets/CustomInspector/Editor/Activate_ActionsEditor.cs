using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Activate_Actions))]
public class Activate_ActionsEditor : Editor
{
    private Activate_Actions source;
    SerializedProperty s_customGameObjects;

    private void OnEnable()
    {
        source = (Activate_Actions)target;
        s_customGameObjects = serializedObject.FindProperty("customGameObjects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if(GUILayout.Button("Add Entry"))
        {
            s_customGameObjects.InsertArrayElementAtIndex(s_customGameObjects.arraySize);
        }

        DrawCustomObjectFields(s_customGameObjects);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCustomObjectFields(SerializedProperty _customList)
    {
        // Loop through custom object list
        for (int i = 0; i < _customList.arraySize; i++)
        {
            GUILayout.BeginHorizontal("box");

            GUILayout.BeginVertical();

EditorGUILayout.PropertyField(_customList.GetArrayElementAtIndex(i).FindPropertyRelative("customGO"), new GUIContent("GameObject: "));

            EditorGUILayout.PropertyField(_customList.GetArrayElementAtIndex(i).FindPropertyRelative("activateTime"), new GUIContent("Time to activate: "));

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            EditorGUILayout.PropertyField(_customList.GetArrayElementAtIndex(i).FindPropertyRelative("activeStatus"), GUIContent.none,GUILayout.Width(25f));

            if(GUILayout.Button("x", GUILayout.Width(20f)))
            {
                s_customGameObjects.DeleteArrayElementAtIndex(i);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }
}
