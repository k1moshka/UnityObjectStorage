using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityStaticData;

/// <summary>
/// Окно для создания схемы объекта, его полей и т.д.
/// </summary>
public class ObjectSchemeWindow : EditorWindow
{
    private Vector2 scrollPos;

    private int selectedStorageType;
    private int selectedDataType;

    private int fieldsCount; // количество полей в схеме

    // fields types
    private List<int> selectsType;
    private string[] availableTypes = Settings.GetRegisteredTypes();
    // field names
    private List<string> fieldKeys;
    // комобобокс о всеми созданными схемами
    private string[] allSchemeNames;
    private int selectedSchemeName;

    // имя схемы данных
    private string schemeName;
    // последнее правильное имя схемы
    private string lastValidSchemeName;

    // gui render
    public void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical();
        if (GUILayout.Button("Reload schemes")) ReloadStorage();
        GUILayout.Label(dataScheme.TypeName + " - change object scheme"); // header

        var selected = EditorGUILayout.Popup("All schemes", selectedSchemeName, allSchemeNames);

        if (selected != selectedSchemeName)
        {
            selectedSchemeName = selected;

            if (selectedSchemeName == allSchemeNames.Length - 1) // if selected "create new.."
                CreateScheme();
            else
                LoadScheme(allSchemeNames[selectedSchemeName]);
        }

        schemeName = EditorGUILayout.TextField("Scheme Name:", schemeName); // change scheme name

        var newIndex = EditorGUILayout.Popup("Inherits from: ", inhIndex, registeredSchemes); // combo inhertitance type
        if (newIndex != inhIndex)
        {
            dataScheme.InheritanceType = registeredSchemes[newIndex];
            inhIndex = newIndex;
        }

        GUILayout.Label("Data fields");

        if (GUILayout.Button("Add scheme field")) // button add fields to scheme
        {
            fieldsCount++;
            selectsType.Add(0);

            var key = dataScheme.AddField(new Field() { Type = Settings.GetDescriptor(availableTypes[0]) });
            fieldKeys.Add(key);
        }

        for (int i = 0; i < fieldsCount; i++) // begin fields render
        {
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            Field field = null;
            try
            {
                field = dataScheme.Fields[fieldKeys[i]];
            } 
            catch (KeyNotFoundException)
            {
                // чтото пошло не так в общем случае это перекомпиляция проекта
                ReloadStorage();
                break;
            }

            // handle rename field
            field.Name = EditorGUILayout.TextField("Field Name:", field.Name);

            // handle change type of field 
            var lastSelectedType = selectsType[i];
            selectsType[i] = EditorGUILayout.Popup("Field Type:", selectsType[i], availableTypes);
            if (lastSelectedType != selectsType[i])
                field.Type = Settings.GetDescriptor(availableTypes[selectsType[i]]);   

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Remove")) RemoveField(i);
            EditorGUILayout.EndHorizontal();
        }                                       // end field render
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save scheme")) SaveScheme(); // button save current scheme
        if (GUILayout.Button("Remove scheme")) RemoveScheme(); // button remove current scheme
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate sources")) GenerateAll();
        if (GUILayout.Button("Regenerate EntityBase")) EntitySourceGenerator.GenerateEntityBase(true);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Assets/Create new object storage...")]
    public static void ShowWindow()
    {
        var window = CreateInstance<ObjectSchemeWindow>();
        window.Show();
    }

    #region window events
    public void Awake()
    {
        DataRegister.Prepare();
        initRegisteredSchemesCombo();
        initInheritanceCombo();

        schemeName = dataScheme.TypeName;
    }
    #endregion

    //------------- public interface
    #region main workflow
    private DataScheme dataScheme;

    private void LoadScheme(string schemeName)
    {
        dataScheme = SchemeStorage.GetScheme(schemeName);
        this.schemeName = this.lastValidSchemeName = dataScheme.TypeName;

        initFields();
        initInheritanceCombo();
    }

    private void CreateScheme()
    {
        dataScheme = new DataScheme();

        initFields();
        initInheritanceCombo();

        schemeName = dataScheme.TypeName = "<define scheme name>";
    }

    private void SaveScheme()
    {
        dataScheme.TypeName = schemeName;

        string message;
        if (!dataScheme.IsValid(out message))
        {
            schemeName = dataScheme.TypeName = lastValidSchemeName;
            throw new Exception("Not valid data scheme: " + message);
        }

        lastValidSchemeName = dataScheme.TypeName;
        dataScheme.RaiseChanged();

        if (!SchemeStorage.HasScheme(dataScheme.TypeName))
            SchemeStorage.AddScheme(dataScheme);

        SchemeStorage.SaveAtProject();

        initInheritanceCombo();
    }

    private void RemoveScheme()
    {
        SchemeStorage.RemoveScheme(dataScheme.TypeName);
        SchemeStorage.SaveAtProject();

        initRegisteredSchemesCombo(true);
        initInheritanceCombo();
    }

    private void RemoveField(int index)
    {
        dataScheme.RemoveField(fieldKeys[index]);

        fieldsCount--;
        fieldKeys.RemoveAt(index);
        selectsType.RemoveAt(index);
    }

    private void ReloadStorage()
    {
        SchemeStorage.ReloadStorage();
        initRegisteredSchemesCombo();
        initInheritanceCombo();

        Debug.Log(string.Format("reloaded", new object[] {   }));
    }

    private void GenerateAll()
    {
        EntitySourceGenerator.GenerateEntityBase();

        foreach (var scheme in SchemeStorage.AllSchemes)
        {
            EntitySourceGenerator.GenerateEntity(scheme);
        }

        AssetDatabase.Refresh();
        this.Close();
    } 
    #endregion

    #region gui helpers

    // инициализация интерфейса для полей схемы
    private void initFields()
    {
        fieldsCount = dataScheme.Fields.Count;

        fieldKeys = new List<string>(dataScheme.Fields.Keys);

        selectsType = new List<int>();
        foreach (var t in dataScheme.Fields.Values)
        {
            selectsType.Add(Array.IndexOf<string>(availableTypes, t.Type.ToString()));
        }
    }

    private void initRegisteredSchemesCombo(bool isLoadFromStorage = true)
    {
        var lastCount = (allSchemeNames != null) ? allSchemeNames.Length : 0; // detect last

        var tempList = new List<string>(SchemeStorage.GetAllRegisteredSchemes());
        tempList.Add("Add new...");
        allSchemeNames = tempList.ToArray();

        selectedSchemeName = 0;

        if (isLoadFromStorage)
        {
            if (tempList.Count > 1)
                LoadScheme(tempList[0]);
            else
                CreateScheme();
        }
        else if (lastCount < allSchemeNames.Length) // if added new scheme, else currentSelected dont change
        {
            selectedSchemeName = allSchemeNames.Length - 2;
        }
        else
        {
            initFields();
        }
    }

    private int inhIndex;
    private string[] registeredSchemes;
    // инициализация комобобокса которыйпоказывает наследуюмую сущность по дефолту EntityBase
    private void initInheritanceCombo()
    {
        var tempList = new List<string>();
        tempList.Add("EntityBase");
        tempList.AddRange(SchemeStorage.GetAllRegisteredSchemes());
        tempList.Remove(dataScheme.TypeName);

        registeredSchemes = tempList.ToArray();
        inhIndex = Array.IndexOf<string>(registeredSchemes, dataScheme.InheritanceType);
    }
    #endregion
}
