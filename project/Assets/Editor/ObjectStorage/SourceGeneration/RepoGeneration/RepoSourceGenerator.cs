using System;
using System.IO;

namespace UnityStaticData
{
    public static partial class RepoSourceGenerator
    {
        private const string SOURCE_FILE_NAME = "Repository.cs";

        private static IRepoGenerator sourceGenerator = new StaticSourceRepositoryGenerator();
        private static IResourceGenerator resourceGenerator = new CompilerResourceGenerator();

        /// <summary>
        /// Генерирование сурсов репозитория и ресурсов для репозитория для всех схем проекта
        /// </summary>
        /// <param name="pathForSources">Путь для сохранения сурсов репозитория</param>
        /// <param name="pathForResources">Путь для сохранения ресурсов репозитория</param>
        /// <param name="pathToAssembly">Путь к скомпилированной сборке содержащей все скомпилированные классы передаваемых схем</param>
        public static void GenerateRepo(
            string pathForSources, 
            string pathForResources, 
            string pathToAssembly)
        {
            resourceGenerator.GenerateResourceRepository(pathForResources, pathToAssembly, SchemeStorage.AllSchemes);

            var repoSource = sourceGenerator.GenerateRepo(SchemeStorage.GetAllRegisteredSchemes(), pathForResources);

            var repoSourcePath = Path.Combine(pathForSources, SOURCE_FILE_NAME);

            if (File.Exists(repoSourcePath))
                File.Delete(repoSourcePath);

            if (!Directory.GetParent(repoSourcePath).Exists)
                Directory.GetParent(repoSourcePath).Create();

            using (var writer = File.CreateText(repoSourcePath))
                writer.Write(repoSource);
        }
    }
}
