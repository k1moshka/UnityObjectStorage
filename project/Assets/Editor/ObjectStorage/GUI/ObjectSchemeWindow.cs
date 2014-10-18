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
    private int selectedStorageType;
    private int selectedDataType;

    private int fieldsCount; // количество полей в схеме

    // fields types
    private List<int> selectsType;
    private string[] avaibleTypes = new string[] { "int", "string", "float", "double", "decimal", "Vector2", "Vector3", "Vector4", "Quaternion" };

    //fields names
    private List<string> fieldNames;
    private string[] allSchemeNames;
    private int selectedSchemeName;

    // gui render
    public void OnGUI()
    {
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

        dataScheme.TypeName = EditorGUILayout.TextField("Scheme Name:", dataScheme.TypeName); // change scheme name

        EditorGUILayout.BeginHorizontal();
        dataScheme.StorageType =    (StorageType)EditorGUILayout.EnumPopup(dataScheme.StorageType);
        dataScheme.DataType =       (DataType)EditorGUILayout.EnumPopup(dataScheme.DataType);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Data fields");

        if (GUILayout.Button("Add scheme field")) // button add fields to scheme
        {
            fieldsCount++;
            fieldNames.Add(string.Empty);
            selectsType.Add(0);
        }
        for (int i = 0; i < fieldsCount; i++) // begin fields of scheme
        {
            EditorGUILayout.Separator();
            fieldNames[i] = EditorGUILayout.TextField("Field Name:", fieldNames[i]);
            selectsType[i] = EditorGUILayout.Popup("Field Type:", selectsType[i], avaibleTypes);
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save")) SaveScheme(); // button save current scheme
        if (GUILayout.Button("Remove")) RemoveScheme(); // button remove current scheme
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    [MenuItem("Assets/Create new object storage...")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectSchemeWindow));
    }

    #region window events
    public void Awake()
    {
        initRegisteredSchemesCombo();
    }
    #endregion

    //------------- public interface
    #region main workflow
    private DataScheme dataScheme;
    public DataScheme DataScheme { get { return DataScheme; } }

    private void LoadScheme(string schemeName)
    {
        dataScheme = SchemeStorage.GetScheme(schemeName);
        lastSchemeName = dataScheme.TypeName;

        initFields();
    }

    private void CreateScheme()
    {
        dataScheme = new DataScheme();

        initFields();

        lastSchemeName = dataScheme.TypeName = "<define scheme name>";
    }

    private void SaveScheme()
    {
        dataScheme.Fields.Clear();
        for (int i = 0; i < fieldNames.Count; i++)
        {
            dataScheme.Fields[fieldNames[i]] = avaibleTypes[selectsType[i]];
        }

        string message;
        if (!dataScheme.IsValid(out message))
            throw new Exception("Not valid data scheme: " + message);

        if (nameChanged) SchemeStorage.RemoveScheme(lastSchemeName); // remove scheme if schemename changed because scheme not replace but scheme add as new scheme
        SchemeStorage.SaveScheme(dataScheme);
        SchemeStorage.SaveAtProject();

        initRegisteredSchemesCombo(false);
    }

    private void RemoveScheme()
    {
        SchemeStorage.RemoveScheme(dataScheme.TypeName);
        SchemeStorage.SaveAtProject();

        initRegisteredSchemesCombo(false);
    }

    private void ReloadStorage()
    {
        SchemeStorage.ReloadStorage();
        initRegisteredSchemesCombo();
    }
    #endregion

    #region gui helpers
    private string lastSchemeName; // имя схемы при загрузки
    private bool nameChanged { get { return lastSchemeName != dataScheme.TypeName && lastSchemeName != null; } }

    // инициализация интерфейса для полей схемы
    private void initFields()
    {
        fieldsCount = dataScheme.Fields.Count;

        fieldNames = new List<string>(dataScheme.Fields.Keys);

        selectsType = new List<int>();
        foreach (var t in dataScheme.Fields.Values)
        {
            selectsType.Add(Array.IndexOf<string>(avaibleTypes, t));
        }
    }

    private void initRegisteredSchemesCombo(bool isLoadFromStorage = true)
    {
        var lastCount = (allSchemeNames != null) ? allSchemeNames.Length : 0; // detect last

        var tempList = new List<string>(SchemeStorage.GetAllRegisteredSchemes());
        tempList.Add("Add new...");
        allSchemeNames = tempList.ToArray();

        if (isLoadFromStorage)
        {
            selectedSchemeName = 0;

            if (tempList.Count > 1)
                LoadScheme(tempList[0]);
            else
                CreateScheme();

            Debug.Log(string.Format("count:{0}; schemeName:{1}", new object[] { allSchemeNames.Length, dataScheme.TypeName }));
        }
        else if (lastCount != allSchemeNames.Length) // if added new scheme, else currentSelected dont change
        {
            selectedSchemeName = allSchemeNames.Length - 2;
        }
    }
    #endregion
}
