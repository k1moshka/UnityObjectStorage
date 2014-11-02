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
    }
}