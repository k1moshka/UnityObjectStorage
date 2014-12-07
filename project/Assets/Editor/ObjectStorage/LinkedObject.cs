using System;
using System.Linq;
using UnityEngine;

namespace UnityStaticData
{
    /// <summary>
    /// Объект описывающий юнити объект в проекте
    /// </summary>
    [Serializable]
    public class LinkedObject
    {
        /// <summary>
        /// Ссылка на объект
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Объект
        /// </summary>
        [field: NonSerialized]
        public UnityEngine.Object Object;
        /// <summary>
        /// Тип объекта
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// Проверяет является ли тип несериализуемым
        /// </summary>
        /// <param name="typeName">Полное имя типа включая нэймспэйс</param>
        /// <returns></returns>
        public static bool IsLinkedObject(string typeName)
        {
            return Settings.GetLinkedTypes().Contains(typeName);
        }
    }
}
