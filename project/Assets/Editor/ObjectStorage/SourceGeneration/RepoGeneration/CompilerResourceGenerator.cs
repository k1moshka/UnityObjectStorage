using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.IO;

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
        /// <param name="pathToAssembly">Путь до сборки проекта</param>
        /// <param name="schemes">Схемы инстансы которых необходимо сохранить в ресурсах</param>
        public void GenerateResourceRepository(string path, string pathToAssembly, DataScheme[] schemes)
        {
            var fullSource = new StringBuilder("using System;using UnityEngine;");
            fullSource.Append(removeUsings(EntitySourceGenerator.GetGeneratedSource("EntityBase")));
            foreach (var s in schemes)
            {
                var source = EntitySourceGenerator.GetGeneratedSource(s.TypeName);

                if (source != null)
                {
                    fullSource.Append(
                        removeUsings(source)
                    );
                }
                else
                    throw new InvalidOperationException("Entity sources must be generated before generate resources :(");
            }

            var allTypes = Assembly.LoadFile(pathToAssembly).GetTypes();

            var serDict = new Dictionary<string, object[]>();

            foreach (var t in allTypes)
            {
                var constructor = t.GetConstructor(new Type[0]);
                var instances = DataRegister.GetInstances(t.Name);
                var instancesToSerialize = new object[instances.Length];
                var index = 0;

                foreach (var i in instances)
                {
                    var newObj = instancesToSerialize[index] = constructor.Invoke(null);

                    foreach (var f in i.FieldsValues)
                    {
                        t
                            .GetProperty(f.Value.Name)
                            .SetValue(newObj, f.Value.Value, null);
                    }

                    index++;
                }

                serDict.Add(
                    t.Name, 
                    instancesToSerialize.ToArray()
                );
            }

            Serializator.SaveTo<Dictionary<string, object[]>>(path, serDict);
        }

        private string removeUsings(string source)
        {
            var index = source.IndexOf("[Serializable]", 0);

            return source.Substring(
                index,
                source.Length - index                       // удаление using'ов
            );
        }
    }
}
