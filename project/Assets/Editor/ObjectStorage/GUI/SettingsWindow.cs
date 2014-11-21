using UnityEditor;
using UnityEngine;
using UnityStaticData;

/// <summary>
/// Окно для создания схемы объекта, его полей и т.д.
/// </summary>
public class SettingsWindow : EditorWindow
{
    private Vector2 scrollPos;
    private string renderMethodName;
    private string renderMethodType;
    private string typeName;
    private string[] allTypes = Settings.GetRegisteredTypes();
    
    public void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical();

        Settings.Instance.PathToSaveData = EditorGUILayout.TextField("Path to save plugin data", Settings.Instance.PathToSaveData);
        Settings.Instance.PathToSaveSources = EditorGUILayout.TextField("Path to save generated sources", Settings.Instance.PathToSaveSources);
        Settings.Instance.ResourcesFileName = EditorGUILayout.TextField("Name of resources file for repository", Settings.Instance.ResourcesFileName);

        GUILayout.Label("Registered types:");

        foreach (var t in allTypes)
        {
            GUILayout.TextField(t);
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Add custom type");
        typeName = EditorGUILayout.TextField("Full type name:", typeName);
        renderMethodType = EditorGUILayout.TextField("Full type containing render method:", renderMethodType);
        renderMethodName = EditorGUILayout.TextField("Render method name:", renderMethodName);
        if (GUILayout.Button("Register new type..."))
            RegisterCustomType();


        if (GUILayout.Button("Save")) Save();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Edit/USD Settings...")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SettingsWindow>().title = "USD Settings";
    }

    #region API
    public void RegisterCustomType()
    {
        Settings.RegisterNewType(new TypeDescriptor() { TypeName = typeName, RenderMethodType = renderMethodType, RenderMethodName = renderMethodName });

        typeName = "";
        renderMethodType = "";
        renderMethodName = "";
        allTypes = Settings.GetRegisteredTypes();
    }

    public void Save()
    {
        Settings.Save();
    }
    #endregion
}