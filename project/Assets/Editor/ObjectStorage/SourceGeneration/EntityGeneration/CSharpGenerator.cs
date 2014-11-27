using System;
using System.Collections.Generic;
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
        private List<string> toStringProps = new List<string>();

        /// <summary>
        /// Генерирование класса или структуры согласно схеме, возвращает c# код
        /// </summary>
        /// <param name="scheme">Схема данных</param>
        /// <param name="data">Данные для сохранения</param>
        public string GenerateEntity(DataScheme scheme)
        {
            toStringProps.Clear();
            builder.Remove(0, builder.Length);  // clear builder

            if (scheme.Relations.Count > 0)
                builder.Append("using System.Collections.Generic;\r\nusing System.Linq;\r\n");

            builder.Append("using System;\r\n\r\n");
            builder.Append("[Serializable]\r\n");
            builder.Append("public ");
            builder.Append(scheme.DataType.ToString().ToLower());
            builder.Append(' ');
            builder.Append(scheme.TypeName);
            builder.Append(" : ");
            builder.Append(scheme.InheritanceType);
            builder.Append("\r\n{\r\n");        // begin class bracket

            foreach (var kv in scheme.Fields)   // render properties
            {
                builder.Append("    public ");
                builder.Append(kv.Value.Type.TypeName); 
                builder.Append(" ");
                builder.Append(kv.Value.Name);
                builder.Append(" { get; set; }\r\n");

                if (kv.Value.IncludedInToString)
                    toStringProps.Add(kv.Value.Name);
            }                                   // end render properties

            foreach (var r in scheme.Relations) // render relations
            {
                
                switch (r.RelationType)
                {
                    case RelationType.Many:   
                        var privateFieldName = RepoSourceGenerator.GetNameForPrivateField(r.EntityName, true);
                        var indexesName = RepoSourceGenerator.GetNameForIndexes(r.EntityName, true);


                        builder.Append("    private int[] ");   // begin indexes
                        builder.Append(indexesName);
                        builder.Append(";\r\n");                // end indexes
                        builder.Append("    private List<");    // brgin private field
                        builder.Append(r.EntityName);
                        builder.Append("> ");
                        builder.Append(privateFieldName);
                        builder.Append(";\r\n");                // end private field
                        builder.Append("    public List<");     // begin property
                        builder.Append(r.EntityName);
                        builder.Append("> ");
                        builder.Append(r.EntityName);
                        builder.Append("s { get { if (");       // begin get and if
                        builder.Append(privateFieldName);
                        builder.Append(" == null) ");           // end if
                        builder.Append(privateFieldName);       // begin initialization
                        builder.Append(" = new List<"); 
                        builder.Append(r.EntityName);
                        builder.Append(">(Repository.GetSet<");
                        builder.Append(r.EntityName);
                        builder.Append(">(a => ");
                        builder.Append(indexesName);
                        builder.Append(".Contains(a.Id)));");   // end init
                        builder.Append(" return ");
                        builder.Append(privateFieldName);       // return value
                        builder.Append("; } }\r\n");            // end property
                        break;
                    case RelationType.One:
                        var privateField = RepoSourceGenerator.GetNameForPrivateField(r.EntityName, true);
                        var idName = RepoSourceGenerator.GetNameForIndexes(r.EntityName, true);

                        builder.Append("    private int ");
                        builder.Append(idName);
                        builder.Append(";\r\n");
                        builder.Append("    private ");
                        builder.Append(r.EntityName);
                        builder.Append(" ");
                        builder.Append(privateField);
                        builder.Append(";\r\n");
                        builder.Append("    public ");
                        builder.Append(r.EntityName);
                        builder.Append(" ");
                        builder.Append(r.EntityName);
                        builder.Append(" { get { if (");    // begin get and if
                        builder.Append(privateField);
                        builder.Append(" == null) ");       // end if
                        builder.Append(privateField);       // begin initialization
                        builder.Append(" = Repository.Get<");
                        builder.Append(r.EntityName);
                        builder.Append(">(a => ");
                        builder.Append("a.Id == ");
                        builder.Append(idName);
                        builder.Append("); return ");       // end init
                        builder.Append(privateField);       // return value
                        builder.Append("; } }\r\n");  
                        break;
                    default:
                        break;
                }
            }                                // end render relations

            // render ToString() method
            if (toStringProps.Count > 0)
            {
                builder.Append("\r\n\r\n    public override string ToString()\r\n    {\r\n        return \"");
                builder.Append(scheme.TypeName);
                builder.Append(" (\"");
                foreach (var p in toStringProps)
                {
                    builder.Append(" + \"");
                    builder.Append(p);
                    builder.Append("->\" + ");
                    builder.Append(p);
                    builder.Append(".ToString() + \";\"");
                }

                builder.Append(" + \")\";\r\n    }\r\n");
            }


            builder.Append(@"}");            // end class bracket

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
            builder.Append("\r\n{\r\n\tpublic int Id { get; set; }\r\n}");
            return builder.ToString();
        }
    }
}
