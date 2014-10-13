using UnityEditor;
using UnityEngine;
using System;

/// <summary>
/// Окно инициализатора сторэйджа для конкретного типа объектов
/// </summary>
public class DataStorageVisualizatorWindow : EditorWindow
{
    [MenuItem("Assets/Manage Storage...")]
    [ContextMenu("Manage Storage...")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DataStorageVisualizatorWindow));
    }

    public void OnGUI()
    {

    }
}