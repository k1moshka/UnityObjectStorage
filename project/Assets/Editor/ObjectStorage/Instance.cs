using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

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

        public Instance()
        {
            // ctor for deserialization
        }
        /// <summary>
        /// Создание нового пустого экземпляра объекта
        /// </summary>
        /// <param name="scheme">Схема данных для инстанса</param>
        public Instance(DataScheme scheme)
        {
            DataScheme = scheme;
            FieldsValues = new Dictionary<string, Field>();

            foreach (var f in DataScheme.Fields)
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
            foreach (var f in FieldsValues)
            {
                if (f.Value != null)
                    f.Value.RenderField();
            }
        }
        /// <summary>
        /// Синхронизация полей со схемой данных
        /// </summary>
        public void SyncWithScheme()
        {
            DataScheme = SchemeStorage.GetScheme(DataScheme.TypeName);

            helpList.Clear();
            foreach (var f in FieldsValues)
            {
                if (!DataScheme.Fields.ContainsKey(f.Key))
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

        #region helpers
        private readonly static List<string> helpList = new List<string>();
        #endregion
    }
}