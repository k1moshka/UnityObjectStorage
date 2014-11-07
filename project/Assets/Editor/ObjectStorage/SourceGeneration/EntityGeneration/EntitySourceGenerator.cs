using System;
using System.IO;

namespace UnityStaticData
{
    public static class EntitySourceGenerator
    {
        private static IEntityGenerator generator;

        static EntitySourceGenerator()
        {
            generator = new CSharpGenerator();
        }
        /// <summary>
        /// Получение сгенерированного сурса для схемы данных
        /// </summary>
        /// <param name="schemeName">Название схемы данных</param>
        /// <returns></returns>
        public static string GetGeneratedSource(string schemeName)
        {
            var path = Settings.GetPathToSaveSources(schemeName + '.' + generator.SourceExtension);

            if (File.Exists(path))
                return File.ReadAllText(path);

            return null;
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
        /// <summary>
        /// Генерирование и сохранение сурса базового для всех сущностей класса EntityBase
        /// </summary>
        /// <returns></returns>
        public static void GenerateEntityBase(bool deleteIfExists = false)
        {
            var source = generator.GenerateEntityBase();

            var pathToFile = Settings.GetPathToSaveSources("EntityBase." + generator.SourceExtension);
            var directory = Directory.GetParent(pathToFile);

            if (File.Exists(pathToFile))
                if (deleteIfExists) File.Delete(pathToFile);
            else
            {
                if (!directory.Exists)
                    Directory.CreateDirectory(directory.ToString());
            }

            using (var stream = File.CreateText(pathToFile))
                stream.Write(source);
        }
    }
}