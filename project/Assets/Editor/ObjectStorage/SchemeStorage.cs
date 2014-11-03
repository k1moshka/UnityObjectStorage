﻿using System;
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
        /// Происходит когда добавляется или удаляется схема данных. Передает имя схемы
        /// </summary>
        [field: NonSerialized]
        public static event Action<SchemeChangedEventArgs> OnSchemesChanged; 

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
            return (from s in _instance.schemes.Values
                    where s.TypeName == schemeName
                    select s).FirstOrDefault();
        }
        /// <summary>
        /// Получение ключа для схемы по ее имени
        /// </summary>
        /// <param name="schemeName">Имя схемы</param>
        /// <returns></returns>
        public static string GetSchemeKey(string schemeName)
        {
            return (from s in _instance.schemes
                    where s.Value.TypeName == schemeName
                    select s.Key).FirstOrDefault();
        }
        /// <summary>
        /// Сохранение новой схемы в реестре схем
        /// </summary>
        /// <param name="scheme">Новая схема</param>
        /// <returns></returns>
        public static string AddScheme(DataScheme scheme)
        {
            var key = KeyGenerator.GenerateStringKey();
            _instance.schemes[key] = scheme;

            raiseSchemesChanged(scheme.TypeName, true);

            return key;
        }
        /// <summary>
        /// Проверяет есть ли схема с указанным именем в реестре
        /// </summary>
        /// <param name="schemeName">Проверяемое имя схемы</param>
        /// <returns></returns>
        public static bool HasScheme(string schemeName)
        {
            return (from s in AllSchemes
                    where s.TypeName == schemeName
                    select s).Count() > 0;
        }
        /// <summary>
        /// Удаление схемы из хранилища
        /// </summary>
        /// <param name="schemeName">Название схемы</param>
        public static void RemoveScheme(string schemeName)
        {
            if (_instance.schemes.ContainsKey(schemeName))
            {
                _instance.schemes[schemeName].CleanUpHandlers();
                _instance.schemes.Remove(schemeName);

                raiseSchemesChanged(schemeName, false);
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
            return (from name in _instance.schemes.Values
                    select name.TypeName).ToArray();
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

        private static void raiseSchemesChanged(string schemeName, bool isAdding)
        {
            if (OnSchemesChanged != null)
                OnSchemesChanged(new SchemeChangedEventArgs() { SchemeName = schemeName, IsAdding = isAdding });
        }

        public class SchemeChangedEventArgs
        {
            /// <summary>
            /// Имя схемы, которая была добавлена или удалена
            /// </summary>
            public string SchemeName    { get; set; }
            /// <summary>
            /// Показывает была ли добавлена схема. true - добавлена, false - удалена
            /// </summary>
            public bool IsAdding        { get; set; }
        }
    }
}