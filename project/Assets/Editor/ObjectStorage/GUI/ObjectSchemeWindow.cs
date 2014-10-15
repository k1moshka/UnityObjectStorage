using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

/// <summary>
/// Окно для создания схемы объекта, его полей и т.д.
/// </summary>
public class ObjectSchemeWindow : EditorWindow 
{
    // рисование интерфейса
     // TODO: imlement
    private string DataID = "";

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Change object scheme");
        DataID = EditorGUILayout.TextField("Data Id:", DataID);
        GUILayout.Label("Data fields");
        GUILayout.EndVertical();
    }

    [MenuItem("Assets/Create new object storage...")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectSchemeWindow));
    }

//------------- public interface
    private DataScheme dataScheme;
    public DataScheme DataScheme { get { return DataScheme; } }

    public void LoadScheme(string schemeName)
    {

    }

    public void SaveScheme()
    {
        // TODO: save at temp unity storage
    }
}
