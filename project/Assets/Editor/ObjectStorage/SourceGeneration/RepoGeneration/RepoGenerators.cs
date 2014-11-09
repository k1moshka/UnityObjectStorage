using System;
using System.Text;

namespace UnityStaticData
{
    /// <summary>
    /// Класс предоставлющий интерфейс для генерирования сущностей
    /// </summary>
    public static partial class RepoSourceGenerator
    {
        /// <summary>
        /// Генератор для статических сурсов
        /// </summary>
        private class StaticSourceRepositoryGenerator : IRepoGenerator
        {
            /// <summary>
            /// Генерирование сурсов на основе (StorageType.Static)
            /// </summary>
            /// <param name="instances">Инстансы которые нужно хранить в репозитории</param>
            /// <returns></returns>
            public string GenerateRepo(Instance[] instances)
            {
                var dictionaryInitilizer = "{ {0}, new {0}[] { {1} } }"; // иницилизатор репозитория для конкретного типа 0 - тип сущности, 1 - экземпляры сущности
                var targetRepoInitilizer = "new {0}() { {1} },\r\n"; // инициализатор для конкретного репозитория 0 - тип сущности, 1 - значения для свойств и полей сущности

                var source = @"public static class Repository
{
    private static Dictionary<string, EntityBase[]> repos = new Dictionary<string, EntityBase[]>()
    {
        {0}
    };

    public static T Get<T>(int index) where T : EntityBase
    {
        return repos[typeof(T).ToString()][index] as T;
    }

    public static T Get<T>(Func<T, bool> predicate) where T : EntityBase
    {
        var targetRepo = repos[typeof(T).ToString()] as T[];
        return targetRepo.FirstOrDefault(predicate);
    }
}";

                var builder = new StringBuilder();

                foreach (var schemeName in SchemeStorage.GetAllRegisteredSchemes())
                {
                    foreach (var instance in DataRegister.GetInstances(schemeName))
                    {
                        foreach (var fieldValue in instance.FieldsValues)
                        {
                            builder.Append(fieldValue.Value.Name);
                            builder.Append(" = ");
                            //builder.Append(fieldValue.Value.);
                        }
                    }
                }

                return string.Format(source, 1);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private class StaticResourceRepositoryGenerator : IRepoGenerator
        {
            public string GenerateRepo(Instance[] instances)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private class DynamicPlayerPrefsRepositoryGenerator : IRepoGenerator
        {
            public string GenerateRepo(Instance[] instances)
            {
                return null;
            }
        }
    }
}
