using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

/// <summary>
/// Окно для создания схемы объекта, его полей и т.д.
/// </summary>
public class ObjectSchemeWindow : EditorWindow 
{
    [MenuItem("Assets/Create new object storage...")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectSchemeWindow));
    }

    // рисование интерфейса
    private string DataID = "";
    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Change object scheme");
        DataID = EditorGUILayout.TextField("Data Id:", DataID);
        GUILayout.Label("Data fields");
        GUILayout.EndVertical();
    }
}
