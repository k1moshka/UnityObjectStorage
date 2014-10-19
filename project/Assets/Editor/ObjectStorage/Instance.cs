using System;
using System.Reflection;
using System.Collections.Generic;

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
        public Dictionary<string, object> FieldsValues { get; set; }

        private object schemeInstance;

        public Instance()
        {
            // ctor for deserialization
        }

        public Instance(DataScheme scheme)
        {
            DataScheme = scheme;
            if (!DataScheme.IsGenerated)
                SourceGenerator.GenerateEntity(DataScheme);


            var type = Type.GetType(DataScheme.TypeName);
            schemeInstance = type.InvokeMember("", BindingFlags.CreateInstance, null, null, null);
            foreach (var f in DataScheme.Fields)
            {
                var prop = type.GetProperty(f.Key); // получение свойства
                var propType = prop.GetType();

                if (!propType.IsPrimitive)
                {
                    prop.SetValue(schemeInstance, null, null); // set null if is primitive type
                }
            }
        }
    }
}