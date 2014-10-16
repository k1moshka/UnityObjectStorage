using System;
using System.Collections.Generic;


namespace UnityStaticData
{
    /// <summary>
    /// Схема данных
    /// </summary>
    [Serializable]
    public class DataScheme
    {
        // <fieldName, typeName 
        /// <summary>
        /// Все поля схемы, <имя поля, имя типа поля>
        /// </summary>
        public Dictionary<string, string> Fields { get; set; }
        /// <summary>
        /// Тип генерируемых сущностей
        /// </summary>
        public DataType DataType { get; set; }
        /// <summary>
        /// Место хранения данных схемы
        /// </summary>
        public StorageType StorageType { get; set; }
        /// <summary>
        /// Название генерируемого типа для схемы
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Создание нового экземпляра
        /// </summary>
        public DataScheme()
        {
            Fields = new Dictionary<string, string>();
        }
    }
}