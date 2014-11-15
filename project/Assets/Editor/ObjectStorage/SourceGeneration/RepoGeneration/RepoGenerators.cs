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
            public string GenerateRepo(string[] schemeNames, string pathToResources)
            {
                var source = @"using System;
using System.Linq;
using System.Collections.Generic;

public static class Repository
{0}
    private static Dictionary<string, object> rawRepos;
    
    static Repository()
    {0}
        using (var stream = System.IO.File.OpenRead(""{2}""))
        {0}
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            rawRepos = (Dictionary<string, object>)formatter.Deserialize(stream);
        {1}
    {1}

    public static T Get<T>(int index) where T : EntityBase
    {0}
        var key = typeof(T).ToString();

        return (rawRepos[key] as T[])[index] as T;
    {1}

    public static T Get<T>(Func<T, bool> predicate) where T : EntityBase
    {0}
        var key = typeof(T).ToString();

        var targetRepo = rawRepos[key] as T[];
        return targetRepo.FirstOrDefault(predicate);
    {1}
{1}";

                return string.Format(source, '{', '}', pathToResources);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private class StaticResourceRepositoryGenerator : IRepoGenerator
        {
            public string GenerateRepo(string[] schemeNames, string pathToResources)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private class DynamicPlayerPrefsRepositoryGenerator : IRepoGenerator
        {
            public string GenerateRepo(string[] schemeNames, string pathToResources)
            {
                return null;
            }
        }
    }
}
