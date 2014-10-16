using System;

namespace UnityStaticData
{
    /// <summary>
    /// Интерфейс описывающий генератор исходных кодов для классов данных
    /// </summary>
    public interface IStaticDataGenerator
    {
        /// <summary>
        /// Генерирование класса или структуры согласно схеме
        /// </summary>
        /// <param name="scheme">Схема данных</param>
        /// <param name="data">Данные для сохранения</param>
        void GenerateStaticData(DataScheme scheme, object[] data);
    }
}