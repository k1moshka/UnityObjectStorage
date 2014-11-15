using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using UnityStaticData;
using System.Collections.Generic;
using System.IO;

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
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Reload data layer")) Reload();        // button reload instances for scheme
        EditorGUILayout.Separator();

        GUILayout.Label("Choose data scheme:");
        var newIndex = EditorGUILayout.Popup(schemeIndex, allSchemes); // combobox all dataschemes
        if (newIndex != schemeIndex)
        {
            schemeIndex = newIndex;

            loadScheme();
            loadInstances();
        }
        EditorGUILayout.Separator();

        if (GUILayout.Button("Add instance")) // button create new instance
            createNewInstance();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        GUILayout.Label(allSchemes[schemeIndex] + " instances:");

        var index = 0;
        var removeIndex = -1;
        foreach (var i in instances)
        {
            folds[index] = EditorGUILayout.Foldout(folds[index], "Instance");
            if (folds[index])
            {
                i.RenderFields(); // render all fields of instance
                if (GUILayout.Button("Remove")) removeIndex = index;

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            index++;
        }
        
        if (removeIndex != -1)
            instances.RemoveAt(removeIndex);

        if (GUILayout.Button("Save")) saveInstances();              // button save instances for scheme
        if (GUILayout.Button("Generate")) generateInstances();      // button generate sources

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }
//////////////////////////////////////////////////////////////////////////////////////////////
    #region main workflow
    public void Reload()
    {
        SchemeStorage.OnSchemesChanged -= SchemeStorage_OnSchemesChanged;

        Awake();
    }

    private void saveInstances()
    {
        if (dataScheme != null)
            DataRegister.SaveInstances(dataScheme.TypeName, instances.ToArray());
    }

    private void generateInstances()
    {
        RepoSourceGenerator.GenerateRepo(
            Settings.Instance.PathToSaveSources,
            "Assets/Resource/sample.bin",
            "obj/Debug/Assembly-CSharp.dll"
            );
    }

    private void createNewInstance()
    {
        instances.Add(new Instance(dataScheme));
        folds.Add(true);
    }
    #endregion
//////////////////////////////////////////////////////////////////////////////////////////////
    #region window events
    public void Awake()
    {
        fillSchemesCombobox();
        loadScheme();
        loadInstances();

        SchemeStorage.OnSchemesChanged += SchemeStorage_OnSchemesChanged;
    }

    public void OnDestroy()
    {
        dataScheme.OnFieldsChanged -= dataScheme_OnChanged;
        SchemeStorage.OnSchemesChanged -= SchemeStorage_OnSchemesChanged;

        DataRegister.Save();
    }
    #endregion
//////////////////////////////////////////////////////////////////////////////////////////////
    #region gui helpers
    private Vector2 scrollPos;
    private List<bool> folds = new List<bool>(); // для фолдаутов

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
        if (dataScheme != null)
        {
            dataScheme.OnFieldsChanged -= dataScheme_OnChanged;
            dataScheme.OnRenameScheme -= dataScheme_OnRenameScheme;
        }

        if (allSchemes.Length > 0)
        {
            dataScheme = SchemeStorage.GetScheme(allSchemes[schemeIndex]);
            dataScheme.OnFieldsChanged += dataScheme_OnChanged;
            dataScheme.OnRenameScheme += dataScheme_OnRenameScheme;
        }
    }

    private List<Instance> instances = new List<Instance>();
    // загрузка текущих данных для выбранной схемы
    private void loadInstances()
    {
        if (dataScheme != null)
        {
            instances.Clear();
            instances.AddRange(DataRegister.GetInstances(dataScheme.TypeName));

            folds.Clear();
            folds.AddRange(new bool[instances.Count]);
        }
    }
    #endregion
//////////////////////////////////////////////////////////////////////////////////////////////
    #region event handlers
    // добавление или удаление схем из контекста
    private void SchemeStorage_OnSchemesChanged(SchemeStorage.SchemeChangedEventArgs args)
    {
        fillSchemesCombobox();

        if (!allSchemes.Contains(dataScheme.TypeName))
        {
            dataScheme.CleanUpHandlers();
            DataRegister.RemoveInstances(dataScheme.TypeName);
            DataRegister.Save();

            schemeIndex = 0;

            loadScheme();
            loadInstances();
        }
    }

    // детект изменения текущей схемы
    private void dataScheme_OnChanged(string schemeName)
    {
        loadScheme();
        loadInstances(); 
    }

    private void dataScheme_OnRenameScheme(string lastName, string newName)
    {
        fillSchemesCombobox();
    }
    #endregion
}