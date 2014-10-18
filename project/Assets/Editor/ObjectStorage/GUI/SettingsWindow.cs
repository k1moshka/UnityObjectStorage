using UnityEditor;
using UnityEngine;
using UnityStaticData;

/// <summary>
/// Окно для создания схемы объекта, его полей и т.д.
/// </summary>
public class SettingsWindow : EditorWindow
{
    private Vector2 scrollPos;

    public void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical();

        Settings.Instance.PathToSaveData = EditorGUILayout.TextField("Path to save plugin data", Settings.Instance.PathToSaveData);
        Settings.Instance.PathToSaveSources = EditorGUILayout.TextField("Path to save generated sources", Settings.Instance.PathToSaveSources);
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
    public void Save()
    {
        Settings.Save();
    }
    #endregion
}