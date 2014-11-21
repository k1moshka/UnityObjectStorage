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
            instanceFolds[index] = EditorGUILayout.Foldout(instanceFolds[index], "Instance");
            if (instanceFolds[index])
            {
                i.RenderFields(); // render all fields of instance

                EditorGUILayout.BeginVertical(GUI.skin.box);
                relFolds[index] = EditorGUILayout.Foldout(relFolds[index], "Related entities:");
                if (relFolds[index])
                    i.RenderRelations(potentialRelations); // render relations
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Remove")) removeIndex = index;

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            index++;
        }

        if (removeIndex != -1)
        {
            instances.RemoveAt(removeIndex);
            calculateIds();
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
            "obj/Debug/Assembly-CSharp.dll"
         );
    }

    private void createNewInstance()
    {
        instances.Add(new Instance(dataScheme));

        calculateIds();

        instanceFolds.Add(true);
        relFolds.Add(false);
    }

    private void calculateIds()
    {
        var index = 0;
        foreach (var i in instances)
        {
            i.FieldsValues["id"].Value = index++;
        }
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

        loadPotentialRelations();
    }

    private List<Instance> instances = new List<Instance>();
    private List<bool> instanceFolds = new List<bool>(); // для фолдаутов инстансов
    private List<bool> relFolds = new List<bool>(); // для фолдаутов связанных сущностей
    // загрузка текущих данных для выбранной схемы
    private void loadInstances()
    {
        if (dataScheme != null)
        {
            instances.Clear();
            instances.AddRange(DataRegister.GetInstances(dataScheme.TypeName));

            instanceFolds.Clear();
            instanceFolds.AddRange(new bool[instances.Count]);

            relFolds.Clear();
            relFolds.AddRange(new bool[instanceFolds.Count]);
        }
    }

    private string[][] potentialRelations;
    private void loadPotentialRelations()
    {
        var tempList = new List<string[]>();
        foreach (var r in dataScheme.Relations)
        {
            var instances = DataRegister.GetInstances(r.EntityName);
            var strs = new string[instances.Length];

            for (int i = 0; i < instances.Length; i++)
            {
                strs[i] = instances[i].ToString();
            }
            tempList.Add(strs);
        }
        potentialRelations = tempList.ToArray();
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