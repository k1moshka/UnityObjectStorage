﻿using System;
using System.IO;

namespace UnityStaticData
{
    public static class SourceGenerator
    {
        private static IEntityGenerator generator;

        static SourceGenerator()
        {
            generator = new CSharpGenerator();
        }
        /// <summary>
        /// Генерирование и сохранение сурса для схемы данных
        /// </summary>
        /// <param name="scheme">Имя схемы данных</param>
        public static void GenerateEntity(DataScheme scheme)
        {
            var source = generator.GenerateEntity(scheme);
            var pathToFile = Settings.GetPathToSaveSources(scheme.TypeName + "." + generator.SourceExtension);

            var directory = Directory.GetParent(pathToFile);

            if (File.Exists(pathToFile))
                File.Delete(pathToFile);
            else
            {
                if (!directory.Exists)
                    Directory.CreateDirectory(directory.ToString());
            }

            using (var stream = File.CreateText(pathToFile))
                stream.Write(source);
        }
        /// <summary>
        /// Возвращение пути до файла
        /// </summary>
        /// <param name="schemeName">Название схемы</param>
        /// <returns></returns>
        public static string GetSourcePath(string schemeName)
        {
            return Settings.GetPathToSaveSources(schemeName + "." + generator.SourceExtension);
        }
    }
}