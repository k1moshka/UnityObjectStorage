using System;
using System.Collections.Generic;
using System.Linq;

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

        private static SchemeStorage _instance;
        private static string PathForSaving { get { return Settings.GetPathToSaveData("schemesStorage.bin"); } }

        /// <summary>
        /// Происходит когда добавляется или удаляется схема данных.
        /// </summary>
        public static event Action OnSchemesChanged;

        public static DataScheme[] AllSchemes { get { return _instance.schemes.Values.ToArray(); } }
        
        static SchemeStorage()
        {
            _instance = loadFromDisk();
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
        public static void SaveScheme(DataScheme scheme)
        {
            var needRaise = false;
            if (!_instance.schemes.ContainsKey(scheme.TypeName))
                needRaise = true;

            _instance.schemes[scheme.TypeName] = scheme;

            if (needRaise)
                raiseOnSchemesChanged();
        }
        /// <summary>
        /// Удаление схемы из хранилища
        /// </summary>
        /// <param name="schemeName">Название схемы</param>
        public static void RemoveScheme(string schemeName)
        {
            if (_instance.schemes.ContainsKey(schemeName))
            {
                _instance.schemes.Remove(schemeName);

                raiseOnSchemesChanged();
            }
        }
        /// <summary>
        /// Сохранение всех схем в папке проекта
        /// </summary>
        public static void SaveAtProject()
        {
            Serializator.SaveTo<SchemeStorage>(
                PathForSaving, 
                _instance);
        }
        /// <summary>
        /// Возвращает все имена зарегистрированных схем
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllRegisteredSchemes()
        {
            return _instance.schemes.Keys.ToArray();
        }
        /// <summary>
        /// Обновление хранилища, синхранизация с текщей версией на диске
        /// </summary>
        public static void ReloadStorage()
        {
            _instance = loadFromDisk();
        }
        /// <summary>
        /// Загрузка с диска текущей версии хранилища
        /// </summary>
        /// <returns></returns>
        private static SchemeStorage loadFromDisk()
        {
            try
            {
                return Serializator.LoadFrom<SchemeStorage>(PathForSaving);
            }
            catch (System.IO.FileNotFoundException)
            {
                return new SchemeStorage();
            }
        }

        private static void raiseOnSchemesChanged()
        {
            if (OnSchemesChanged != null)
                OnSchemesChanged();
        }
    }
}