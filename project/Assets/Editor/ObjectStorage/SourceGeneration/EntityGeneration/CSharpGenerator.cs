﻿using System;
using System.Text;

namespace UnityStaticData
{
    /// <summary>
    /// Генератор сурсов, который создает исходный код на c#
    /// </summary>
    public class CSharpGenerator : IEntityGenerator
    {
        private StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Генерирование класса или структуры согласно схеме, возвращает c# код
        /// </summary>
        /// <param name="scheme">Схема данных</param>
        /// <param name="data">Данные для сохранения</param>
        public string GenerateEntity(DataScheme scheme)
        {
            builder.Remove(0, builder.Length); // clear builder

            builder.Append("using System;\n\n");
            builder.Append("[Serializable]\n");
            builder.Append("public ");
            builder.Append(scheme.DataType.ToString().ToLower());
            builder.Append(' ');
            builder.Append(scheme.TypeName);
            builder.Append(" : EntityBase");
            builder.Append("\n{\n"); // begin class bracket

            foreach (var kv in scheme.Fields) // render properties
            {
                builder.Append("    public ");
                builder.Append(kv.Value.Type.TypeName); 
                builder.Append(" ");
                builder.Append(kv.Key);
                builder.Append(" { get; set; }\n");
            }

            builder.Append(@"}"); // end class bracket

            return builder.ToString();
        }
        /// <summary>
        /// Расширение файла сурса
        /// </summary>
        public string SourceExtension
        {
            get { return "cs"; }
        }
        /// <summary>
        /// Генерирование EntityBase класса
        /// </summary>
        /// <returns></returns>
        public string GenerateEntityBase()
        {
            var source = @"using System;

public {0} EntityBase
{
    public int Index;
}";
            return string.Format(source, scheme.DataType.ToString().ToLower());
        }
    }
}
