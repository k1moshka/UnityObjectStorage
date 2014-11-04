using System;
using System.Collections.Generic;

namespace UnityStaticData
{
    /// <summary>
    /// Промежуточный реестр данных
    /// </summary>
    public static class DataRegister
    {
        private const string FILENAME = "inst.bin";
        // <dataSchemeName, instances of datascheme>
        private static Dictionary<string, Instance[]> instances;

        static DataRegister()
        {
            try
            {
                instances = Serializator.LoadFrom<Dictionary<string, Instance[]>>(Settings.GetPathToSaveData(FILENAME));
            }
            catch (System.IO.FileNotFoundException)
            {
                instances = new Dictionary<string, Instance[]>();
            }

            foreach (var s in SchemeStorage.GetAllRegisteredSchemes())
            {
                // отсоеденение производлится автоматически при удалении схемы из репозитория схем SchemeStorage
                SchemeStorage.GetScheme(s).OnFieldsChanged += SyncSchemeInstances;
            }

            SchemeStorage.OnSchemesChanged += SchemeStorage_OnSchemesChanged;
        }
        /// <summary>
        /// Инициализация реестра данных
        /// </summary>
        public static void Prepare()
        {
            // используется для того что бы вызвать статический конструктор
        }
        /// <summary>
        /// Сохранение всех инстансов для всех схем в папке проекта на диске.
        /// </summary>
        public static void Save()
        {
            Serializator.SaveTo<Dictionary<string, Instance[]>>(
                Settings.GetPathToSaveData(FILENAME),
                instances);
        }
        /// <summary>
        /// Получение всех инстансов для схемы данных
        /// </summary>
        /// <param name="dataSchemeName">Имя схемы данных</param>
        /// <returns></returns>
        public static Instance[] GetInstances(string dataSchemeName)
        {
            var refferenceKey = SchemeStorage.GetSchemeKey(dataSchemeName);

            if (!DataRegister.instances.ContainsKey(refferenceKey))
                instances[refferenceKey] = new Instance[0];

            return instances[refferenceKey];
        }
        /// <summary>
        /// Сохранение инстансов для схемы данных
        /// </summary>
        /// <param name="dataSchemeName">Имя схемы данных</param>
        /// <param name="instancesToSave">Инстансы для сохранения</param>
        public static void SaveInstances(string dataSchemeName, Instance[] instancesToSave)
        {
            var refferenceKey = SchemeStorage.GetSchemeKey(dataSchemeName);

            instances[refferenceKey] = instancesToSave;
        }
        /// <summary>
        /// Удаление инстансов для схемы данных
        /// </summary>
        /// <param name="dataSchemeName">Имя схемы</param>
        public static void RemoveInstances(string dataSchemeName)
        {
            var refferenceKey = SchemeStorage.GetSchemeKey(dataSchemeName);

            if (instances.ContainsKey(refferenceKey))
            {
                instances.Remove(refferenceKey);
            }
        }
        /// <summary>
        /// Приведение всех инстансов к текущему состоянию связанной схемы
        /// </summary>
        /// <param name="schemeName">Имя схемы для которой нужно провести валидацию</param>
        public static void SyncSchemeInstances(string schemeName)
        {
            foreach (var instance in GetInstances(schemeName))
            {
                instance.SyncWithScheme();
            }

            Save();
        }

        private static void SchemeStorage_OnSchemesChanged(SchemeStorage.SchemeChangedEventArgs args)
        {
            if (args.IsAdding)
            {
                SchemeStorage.GetScheme(args.SchemeName).OnFieldsChanged += SyncSchemeInstances;
            }
            else
            {
                RemoveInstances(args.SchemeName);
            }
        }  
    }
}