using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace UnityStaticData
{
    /// <summary>
    /// Представление экземпляра сущности для схемы данных, необходимый для генерации репозитория или ресурсов
    /// </summary>
    [Serializable]
    public class Instance
    {
        /// <summary>
        /// Схема для экземпляра
        /// </summary>
        public DataScheme DataScheme { get; set; }
        /// <summary>
        /// Значение для полей экземпляра   
        /// </summary>
        public Dictionary<string, Field> FieldsValues { get; set; }
        /// <summary>
        /// Значения для связанных сущностей
        /// </summary>
        public Dictionary<string, int[]> Relations { get; set; }

        public Instance()
        {
            // ctor for deserialization
            Relations = new Dictionary<string, int[]>();
        }
        /// <summary>
        /// Создание нового пустого экземпляра объекта
        /// </summary>
        /// <param name="scheme">Схема данных для инстанса</param>
        public Instance(DataScheme scheme)
            :this()
        {
            DataScheme = scheme;
            FieldsValues = new Dictionary<string, Field>();

            FieldsValues.Add("id", new Field(Settings.GetDescriptor("int")) { Name = "Id" });

            foreach (var f in getFields(DataScheme.TypeName))
            {
                FieldsValues.Add(f.Key, new Field(f.Value.Type) { Name = f.Value.Name });
            }
        }
        /// <summary>
        /// Отрисовка всех полей для экземпляра объекта схемы данных
        /// </summary>
        /// <returns></returns>
        public void RenderFields()
        {
            if (FieldsValues != null)
            {
                foreach (var f in FieldsValues)
                {
                    if (f.Value != null && f.Key != "id")
                        f.Value.RenderField();
                }
            }
        }
        /// <summary>
        /// Отрисовка полей связей для экземпляра семы данных
        /// </summary>
        public void RenderRelations(string[][] potentialRelations)
        {
            if (DataScheme.Relations != null)
            {
                for (int i = 0; i < DataScheme.Relations.Count; i++)
                {
                    GUILayout.Label("Relation to " + DataScheme.Relations[i].EntityName + " [type: " + DataScheme.Relations[i].RelationType.ToString() + "]");

                    if (indexes.Count - 1 < i)
                        indexes.Add(new List<int>());

                    var canAddNew = false;

                    if (DataScheme.Relations[i].RelationType == RelationType.Many)
                    {
                        for ( var j = 0; j < indexes[i].Count; j++ )
                        {
                            indexes[i][j] = EditorGUILayout.Popup("", indexes[i][j], potentialRelations[i]);
                        }
                        canAddNew = true;
                    }

                    EditorGUILayout.BeginHorizontal();
                    GUI.enabled = canAddNew;
                    if (GUILayout.Button("Add")) { indexes[i].Add(0); }
                    if (GUILayout.Button("Remove")) { indexes[i].RemoveAt(indexes[i].Count - 1); }
                    GUI.enabled = true;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator();
                }
            }
            EditorGUILayout.Separator();
        }
        /// <summary>
        /// Сохранение связей инстанса с другими сущностями
        /// </summary>
        public void SaveRelations()
        {
            Relations.Clear();
            for (int i = 0; i < indexes.Count; i++)
            {
                Relations.Add(DataScheme.Relations[i].EntityName, indexes[i].ToArray());
            }
        }
        /// <summary>
        /// Синхронизация полей со схемой данных
        /// </summary>
        public void SyncWithScheme()
        {
            if (DataScheme == null)
                return;

            DataScheme = SchemeStorage.GetScheme(DataScheme.TypeName);

            if (DataScheme == null)
                return;

            helpList.Clear();
            foreach (var f in FieldsValues)
            {
                if (!DataScheme.Fields.ContainsKey(f.Key) || DataScheme.Fields[f.Key].Type.TypeName != f.Value.Type.TypeName)
                {
                    helpList.Add(f.Key); // add field for delete
                }
            }

            // delete all marked fields
            foreach (var d in helpList)
            {
                FieldsValues.Remove(d);
            }
            helpList.Clear();

            // add new fields and rename containing fields
            foreach (var f in DataScheme.Fields)
            {
                if (!FieldsValues.ContainsKey(f.Key))
                {
                    FieldsValues.Add(f.Key, new Field(f.Value.Type) { Name = f.Value.Name });
                }
                else
                {
                    FieldsValues[f.Key].Name = f.Value.Name;
                }
            }
        }

        public override string ToString()
        {
            if (FieldsValues == null)
                return base.ToString();

            if (DataScheme != null
                && DataScheme.MainFieldKey != null
                && FieldsValues[DataScheme.MainFieldKey] != null
                && FieldsValues[DataScheme.MainFieldKey].Value != null
                )
                return toStringRepresentation(FieldsValues[DataScheme.MainFieldKey]);

            if (DataScheme != null && FieldsValues.Count > 0)
            {
                var first = FieldsValues.First();
                if (first.Value != null && first.Value.Value != null)
                    return toStringRepresentation(first.Value);
            }

            return base.ToString();
        }

        #region helpers
        private readonly static List<string> helpList = new List<string>();
        // рекурсивное получение всех свойств класса
        private Dictionary<string, Field> getFields(string schemeName)
        {
            var result = new Dictionary<string, Field>();
            var dataScheme = SchemeStorage.GetScheme(schemeName);
            if (dataScheme != null)
            {
                foreach (var kv in getFields(dataScheme.InheritanceType))
                {
                    result.Add(kv.Key, kv.Value);
                }
                foreach (var kv in dataScheme.Fields)
                {
                    result.Add(kv.Key, kv.Value);
                }
                return result;
            }
            return new Dictionary<string, Field>();
        }

        private string toStringRepresentation(Field field)
        {
            return DataScheme.TypeName + "->" + field.Name + ": " + field.Value.ToString();
        }
        #endregion

        #region render helper
        private List<List<int>> indexes = new List<List<int>>();
        #endregion
    }
}