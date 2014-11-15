using System;
using System.IO;
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

            builder.Append("using System;\r\n\r\n");
            builder.Append("[Serializable]\r\n");
            builder.Append("public ");
            builder.Append(scheme.DataType.ToString().ToLower());
            builder.Append(' ');
            builder.Append(scheme.TypeName);
            builder.Append(" : ");
            builder.Append(scheme.InheritanceType);
            builder.Append("\r\n{\r\n"); // begin class bracket

            foreach (var kv in scheme.Fields) // render properties
            {
                builder.Append("    public ");
                builder.Append(kv.Value.Type.TypeName); 
                builder.Append(" ");
                builder.Append(kv.Value.Name);
                builder.Append(" { get; set; }\r\n");
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
            builder.Remove(0, builder.Length);

            builder.Append(string.Format("using System;\r\n[Serializable]\r\npublic {0} EntityBase", "class"));
            builder.Append("\r\n{\r\n\tpublic int Index;\r\n}");
            return builder.ToString();
        }
    }
}
