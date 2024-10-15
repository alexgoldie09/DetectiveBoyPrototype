using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorExtensions
{
    public static void DrawActionsArray(SerializedProperty _array, string _label)
    {
        GUILayout.BeginVertical("box");

        EditorGUILayout.LabelField(_label);

        if(_array.arraySize == 0)
        {
            if(GUILayout.Button("Add Actions"))
            {
                _array.InsertArrayElementAtIndex(0);
            }
        }

        for(int i = 0; i < _array.arraySize; i++)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(_array.GetArrayElementAtIndex(i), GUIContent.none);

            if(GUILayout.Button("x", GUILayout.Width(20f)))
            {
                _array.DeleteArrayElementAtIndex(i);
            }

            if(i == _array.arraySize - 1)
            {
                if (GUILayout.Button("+",GUILayout.Width(20f)))
                {
                    _array.InsertArrayElementAtIndex(_array.arraySize);
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
