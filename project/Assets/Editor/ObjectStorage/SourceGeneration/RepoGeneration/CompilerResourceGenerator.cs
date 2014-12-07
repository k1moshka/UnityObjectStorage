using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;
using UnityEditor;

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
            if (EntitySourceGenerator.GetGeneratedSource("EntityBase") == null)
                throw new InvalidOperationException("Entity sources must be generated before generate resources :(");

            var allTypes = Assembly.LoadFile(pathToAssembly).GetTypes();

            var serDict = new Dictionary<string, object>();
            var dataTypes = SchemeStorage.GetAllRegisteredSchemes();

            foreach (var t in allTypes)
            {
                if (!dataTypes.Contains(t.Name))
                    continue;

                var constructor = t.GetConstructor(new Type[0]);

                var instances = DataRegister.GetInstances(t.Name);
                
                var instancesToSerialize = Array.CreateInstance(t, instances.Length);       
                var index = 0;

                var dataType = SchemeStorage.GetScheme(t.ToString());

                foreach (var i in instances)
                {
                    var newObj = constructor.Invoke(null);

                    instancesToSerialize.SetValue(newObj, index);

                    // TODO: добавить проверку не только свойст но и полей (advanced)
                    foreach (var f in i.FieldsValues)
                    {
                        if (LinkedObject.IsLinkedObject(f.Value.Type.TypeName))
                        {
                            Debug.Log(string.Format("typename:{0}", new object[] { f.Value.Type.TypeName }));
                            // TODO: сохранить как строку
                            // TODO: переделать генерацию сурсов сущностей, чтобы свойства создавались но не сериализовались, сериализовался только путь до объекта
                            var sprite = f.Value.Value as LinkedObject;

                            t
                                .GetField(USDUtil.GetLinkName(f.Value.Name), BindingFlags.Instance | BindingFlags.NonPublic)
                                .SetValue(newObj, USDUtil.GetLocalizedPath(sprite.Link));
                        }
                        else
                            t
                                .GetProperty(f.Value.Name)
                                .SetValue(newObj, f.Value.Value, null);
                    }

                    foreach (var kv in i.Relations)
                    {
                        if (kv.Value.Length > 0)
                        {
                            var relationType = kv.Key;
                            var rType = (from __relationType in dataType.Relations
                                         where __relationType.EntityName == relationType
                                         select __relationType.RelationType).FirstOrDefault();

                            var indexesFieldName = USDUtil.GetNameForIndexes(relationType, rType == RelationType.Many);

                            if ((rType == RelationType.One) && kv.Value != null)
                            {
                                t
                                    .GetField(indexesFieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                                    .SetValue(newObj, kv.Value[0]);
                            }
                            else
                            {
                                var ids = kv.Value != null ? kv.Value.ToArray() : new int[0];
                                t
                                    .GetField(indexesFieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                                    .SetValue(newObj, ids);
                            }
                        }
                    }

                    index++;
                }

                serDict.Add(
                    t.Name, 
                    instancesToSerialize
                );
            }

            Serializator.SaveTo<Dictionary<string, object>>(path, serDict);
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
