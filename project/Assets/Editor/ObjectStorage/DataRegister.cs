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
            if (!DataRegister.instances.ContainsKey(dataSchemeName))
                instances[dataSchemeName] = new Instance[0];

            return instances[dataSchemeName];
        }
        /// <summary>
        /// Сохранение инстансов для схемы данных
        /// </summary>
        /// <param name="dataSchemeName">Имя схемы данных</param>
        /// <param name="instancesToSave">Инстансы для сохранения</param>
        public static void SaveInstances(string dataSchemeName, Instance[] instancesToSave)
        {
            instances[dataSchemeName] = instancesToSave;
        }

        public static void RemoveInstances(string dataSchemeName)
        {
            if (instances.ContainsKey(dataSchemeName))
            {
                instances.Remove(dataSchemeName);
            }
        }
        /// <summary>
        /// Приведение всех инстансов к текущему состоянию связанной схемы
        /// </summary>
        /// <param name="schemeName">Имя схемы для которой нужно провести валидацию</param>
        public static void SyncSchemeInstances(string schemeName)
        {
            foreach (var instance in instances[schemeName])
            {
                instance.SyncWithScheme();
            }
        }
    }
}