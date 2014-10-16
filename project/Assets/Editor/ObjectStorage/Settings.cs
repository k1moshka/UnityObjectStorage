using System;
using System.IO;

namespace UnityStaticData
{
    /// <summary>
    /// Класс хранящий и управляющий настройками
    /// </summary>
    [Serializable]
    public class Settings
    {
        private const string DEFAULT_PATH = "Assets/DataLayer/";
        private const string SETTING_PATH = "Assets/Editor/UnityStaticData/s.set";
        /// <summary>
        /// Экземпляр настроек
        /// </summary>
        public static Settings Instance { get; private set; }
        static Settings()
        {
            Instance = loadSettings();
            if (string.IsNullOrEmpty(Instance.PathToSaveData))
            {
                Instance.PathToSaveData = DEFAULT_PATH;
            }
        }
        /// <summary>
        /// Получение валидного пути, для сохранения в настроенной папке проэкта
        /// </summary>
        /// <param name="fileName">Имя файла для сохранения</param>
        /// <returns></returns>
        public static string GetPathToSaveData(string fileName)
        {
            return Instance.PathToSaveData + fileName;
        }
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        public static void Save()
        {
            Serializator.SaveTo<Settings>(SETTING_PATH, Instance);
        }
        /// <summary>
        /// Путь к папке сохранения данных
        /// </summary>
        public string PathToSaveData { get; set; }

        private static Settings loadSettings()
        {
            // загружает их файла если настройки были сохранены на диске
            try
            {
                return Serializator.LoadFrom<Settings>(SETTING_PATH);
            }
            catch (FileNotFoundException)
            {
                return new Settings();
            }
        }
    }
}
