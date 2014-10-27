using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStaticData
{
    /// <summary>
    /// Представление экземпляра сущности для схемы данных, необходимый для генерации репозитория или ресурсов
    /// </summary>
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
                FieldsValues.Add(f.Key, new Field(f.Value));
            }
        }
        /// <summary>
        /// Отрисовка всех полей для экземпляра объекта схемы данных
        /// </summary>
        /// <returns></returns>
        public void RenderFields()
        {
            foreach (var f in FieldsValues)
            {
                f.Value.RenderField(f.Key);
            }
        }
    }
}