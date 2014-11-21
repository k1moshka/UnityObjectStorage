﻿using System;
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
        public static void GenerateRepo(string pathToAssembly)
        {
            resourceGenerator.GenerateResourceRepository(Settings.GetPathToSaveResources(), pathToAssembly, SchemeStorage.AllSchemes);            
        }
        /// <summary>
        /// Генерация сурсов для репозитория
        /// </summary>
        /// <param name="pathForResources"></param>
        public static void GenerateRepoSources()
        {
            var repoSource = sourceGenerator.GenerateRepo(SchemeStorage.GetAllRegisteredSchemes(), Settings.GetPathToSaveResources());

            var repoSourcePath = Settings.GetPathToSaveSources(SOURCE_FILE_NAME);

            if (File.Exists(repoSourcePath))
                File.Delete(repoSourcePath);

            if (!Directory.GetParent(repoSourcePath).Exists)
                Directory.GetParent(repoSourcePath).Create();

            using (var writer = File.CreateText(repoSourcePath))
                writer.Write(repoSource);
        }
        /// <summary>
        /// Возвращает имя для поля класса которое предоставляет связанные индексы
        /// </summary>
        /// <param name="entityName">Имя сущности поля</param>
        /// <param name="isMany">Показывает список сущностей или одна сущность хранится в поле</param>
        /// <returns></returns>
        public static string GetNameForIndexes(string entityName, bool isMany)
        {
            return GetNameForPrivateField(entityName, isMany) + "Indexes";
        }
        /// <summary>
        /// Возвращает имя поля класса которое предоставляет сущности
        /// </summary>
        /// <param name="entityName">Имя сущности поля</param>
        /// <param name="isMany">Показывает список сущностей или одна сущность хранится в поле</param>
        /// <returns></returns>
        public static string GetNameForPrivateField(string entityName, bool isMany = false)
        {
            var result = "_" + entityName.ToLower();
            
            return isMany ? result + "s" : result;
        }
    }
}
