using System;

namespace UnityStaticData
{
    /// <summary>
    /// Интерфейс описывающий генератор репозитория
    /// </summary>
    public interface IRepoGenerator
    {
        /// <summary>
        /// Генерация сурсов для репозитория, на основе переданных инстансов.Возвращает исходный код репозтория
        /// </summary>
        /// <param name="instances">Инстансы для которых нужно создать репозиторий</param>
        /// <returns></returns>
        string GenerateRepo(Instance[] instances);
    }
}
