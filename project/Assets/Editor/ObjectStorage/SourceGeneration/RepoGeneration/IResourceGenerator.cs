using System;

namespace UnityStaticData
{
    /// <summary>
    /// Интерфейс предоставляющий АПИ для генератора ресурсов проекта
    /// </summary>
    public interface IResourceGenerator
    {
        /// <summary>
        /// Генерирование ресурсов на основе переданных схем данных
        /// </summary>
        /// <param name="path">Путь до файла в который сохраняться будут ресурсы</param>
        /// <param name="pathToAssembly">Путь до сборки проекта</param>
        /// <param name="schemes">Схемы инстансы которых необходимо сохранить в ресурсах</param>
        void GenerateResourceRepository(string path, string pathToAssembly, DataScheme[] schemes);
    }
}
