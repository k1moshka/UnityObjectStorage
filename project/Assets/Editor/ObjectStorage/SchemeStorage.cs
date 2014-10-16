using System;
using System.Collections.Generic;

namespace UnityStaticData
{
    /// <summary>
    /// Независимое хранилище схем данных используемое для хранения в постоянном формате схем данных
    /// </summary>
    [Serializable]
    public class SchemeStorage
    {
        /// <summary>
        /// Все схемы проекта
        /// </summary>
        public Dictionary<string, DataScheme> schemes;

        private SchemeStorage()
        {
            schemes = new Dictionary<string, DataScheme>();
        }

        private readonly static SchemeStorage _instance;

        static SchemeStorage()
        {
            _instance = new SchemeStorage();
        }
        /// <summary>
        /// Поучить схему по имени, если таковой нет, то создается новая с таким именем
        /// </summary>
        /// <param name="schemeName">Имя схемы</param>
        /// <returns></returns>
        public static DataScheme GetScheme(string schemeName)
        {
            if (!_instance.schemes.ContainsKey(schemeName) || _instance.schemes[schemeName] == null)
                _instance.schemes[schemeName] = new DataScheme();
            return _instance.schemes[schemeName];
        }
        /// <summary>
        /// Сохранение схемы, в объекте
        /// </summary>
        /// <param name="schemeName"></param>
        /// <param name="scheme"></param>
        public static void SaveScheme(string schemeName, DataScheme scheme)
        {
            _instance.schemes[schemeName] = scheme;
        }
        /// <summary>
        /// Сохранение всех схем в папке проекта
        /// </summary>
        public static void SaveAtProject()
        {
            Serializator.SaveTo<SchemeStorage>(
                Settings.GetPathToSaveData("schemesStorage.bin"), 
                _instance);
        }
    }
}