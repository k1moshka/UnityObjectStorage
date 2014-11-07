using System;

namespace UnityStaticData
{
    /// <summary>
    /// Генератор ресурсов основаный на компиляции сурсов во временную сборку и последующей сериализации
    /// </summary>
    public class CompilerResourceGenerator : IResourceGenerator
    {
        /// <summary>
        /// Генерирование ресурсов на основе переданных схем данных
        /// </summary>
        /// <param name="path">Путь до файла в который сохраняться будут ресурсы</param>
        /// <param name="schemes">Схемы инстансы которых необходимо сохранить в ресурсах</param>
        public void GenerateResourceRepository(string path, DataScheme[] schemes)
        {
            
        }
    }
}
