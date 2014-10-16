using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityStaticData;

/// <summary>
/// Окно для создания схемы объекта, его полей и т.д.
/// </summary>
public class ObjectSchemeWindow : EditorWindow 
{
    // рисование интерфейса
     // TODO: implement
    private string DataID = "";
    private int fieldsCount;

    private List<string> fieldNames;
    private List<string> fieldTypes;

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Change object scheme");
        DataID = EditorGUILayout.TextField("Data Id:", DataID);
        GUILayout.Label("Data fields");

        if (GUILayout.Button("Add scheme field"))
        {
            fieldsCount++;
            fieldNames.Add(string.Empty);
            fieldTypes.Add(string.Empty);
        }
        for (int i = 0; i < fieldsCount; i++)
        {
            EditorGUILayout.Separator();
            fieldNames[i] = EditorGUILayout.TextField("Field Name:", fieldNames[i]);
            fieldTypes[i] = EditorGUILayout.TextField("Field Type:", fieldTypes[i]);
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save")) SaveScheme();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    [MenuItem("Assets/Create new object storage...")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(ObjectSchemeWindow)) as ObjectSchemeWindow;
        window.LoadScheme("sample scheme");
    }

    public void OnDestroy()
    {
        SaveScheme();
    }

//------------- public interface
    private string schemeName;
    private DataScheme dataScheme;
    public DataScheme DataScheme { get { return DataScheme; } }

    public void LoadScheme(string schemeName)
    {
        dataScheme = SchemeStorage.GetScheme(schemeName);
        fieldsCount = dataScheme.Fields.Count;

        fieldNames = new List<string>(dataScheme.Fields.Keys);
        fieldTypes = new List<string>(dataScheme.Fields.Values);

        this.schemeName = schemeName;
    }

    public void SaveScheme()
    {
        SchemeStorage.SaveScheme(schemeName, dataScheme);
        SchemeStorage.SaveAtProject();
    }
}
