using System;

namespace UnityStaticData
{
    /// <summary>
    /// Генератор ключей
    /// </summary>
    public static class KeyGenerator
    {
        /// <summary>
        /// Генерирует новую уникальную строку
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
