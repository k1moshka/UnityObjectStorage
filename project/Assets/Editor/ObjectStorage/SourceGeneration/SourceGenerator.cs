using System;
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

        public static void GenerateEntity(DataScheme scheme)
        {
            var source = generator.GenerateEntity(scheme);
            var pathToFile = Settings.GetPathToSaveSources(scheme.TypeName + ".cs");

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
    }
}