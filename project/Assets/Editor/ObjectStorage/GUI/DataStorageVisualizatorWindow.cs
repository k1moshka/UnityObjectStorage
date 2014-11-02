using UnityEditor;
using UnityEngine;
using System;
using UnityStaticData;
using System.Collections.Generic;

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
        // TODO: реализовать синхронизацию с schemeobjectwindow через контекст GUI

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Reload data layer")) Reload();        // button reload instances for scheme
        EditorGUILayout.Separator();

        var newIndex = EditorGUILayout.Popup(schemeIndex, allSchemes); // combobox all dataschemes
        if (newIndex != schemeIndex)
        {
            schemeIndex = newIndex;

            loadScheme();
            loadInstances();
        }
        EditorGUILayout.Separator();

        if (GUILayout.Button("Add")) // button create new instance
            createNewInstance();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        foreach (var i in instances)
        {
            i.RenderFields(); // render all fields of instance

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

        if (GUILayout.Button("Save")) saveInstances();              // button save instances for scheme
        if (GUILayout.Button("Generate")) generateInstances();      // button generate sources

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }
//////////////////////////////////////////////////////////////////////////////////////////////
    #region main workflow
    public void Reload()
    {
        Awake();
    }

    private void saveInstances()
    {
        if (dataScheme != null)
            DataRegister.SaveInstances(dataScheme.TypeName, instances.ToArray());
    }

    private void generateInstances()
    {
        // TODO: реализовать генерирование сурсов или ресурсов инстансов и конечный апи репозитория + проверка сгенерены ли сурсы для текущей схемы
    }

    private void createNewInstance()
    {
        instances.Add(new Instance(dataScheme));
    }
    #endregion
//////////////////////////////////////////////////////////////////////////////////////////////
    #region window events
    public void Awake()
    {
        fillSchemesCombobox();
        loadScheme();
    }
    #endregion
//////////////////////////////////////////////////////////////////////////////////////////////
    #region gui helpers
    private Vector2 scrollPos;

    // load all schemes from storage
    private string[] allSchemes;
    private int schemeIndex;
    private void fillSchemesCombobox()
    {
        allSchemes = SchemeStorage.GetAllRegisteredSchemes();
    }
    //loading scheme
    private DataScheme dataScheme;
    private void loadScheme()
    {
        if (allSchemes.Length > 0)
            dataScheme = SchemeStorage.GetScheme(allSchemes[schemeIndex]);
    }

    private List<Instance> instances = new List<Instance>();
    // загрузка текущих данных для выбранной схемы
    private void loadInstances()
    {
        if (dataScheme != null)
        {
            instances.Clear();
            instances.AddRange(DataRegister.GetInstances(dataScheme.TypeName));
        }
    }


    #endregion
}