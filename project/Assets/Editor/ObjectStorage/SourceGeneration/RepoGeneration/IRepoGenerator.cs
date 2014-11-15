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
        /// <param name="schemeNames">Инстансы для которых нужно создать репозиторий</param>
        /// <param name="pathToResoures">Путь до файла сериализированных ресурсов</param>
        /// <returns></returns>
        string GenerateRepo(string[] schemeNames, string pathToResoures);
    }
}
