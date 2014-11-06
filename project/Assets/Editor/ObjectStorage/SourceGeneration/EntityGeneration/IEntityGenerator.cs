using System;

namespace UnityStaticData
{
    /// <summary>
    /// Интерфейс описывающий генератор исходных кодов для классов данных
    /// </summary>
    public interface IEntityGenerator
    {
        /// <summary>
        /// Расширение файла сурса
        /// </summary>
        string SourceExtension { get; }
        /// <summary>
        /// Генерирование класса или структуры согласно схеме, возвращает c# код
        /// </summary>
        /// <param name="scheme">Схема данных</param>
        /// <param name="data">Данные для сохранения</param>
        string GenerateEntity(DataScheme scheme);
        /// <summary>
        /// Генерирование EntityBase класса
        /// </summary>
        /// <returns></returns>
        string GenerateEntityBase();
    }
}