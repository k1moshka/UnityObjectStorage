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
    }
}