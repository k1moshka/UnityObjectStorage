using System;
using System.IO;

namespace UnityStaticData
{
    /// <summary>
    /// Класс хранящий и управляющий настройками
    /// </summary>
    [Serializable]
    public partial class Settings
    {
        /// <summary>
        /// Экземпляр настроек
        /// </summary>
        public static Settings Instance { get; private set; }
        static Settings()
        {
            Instance = loadSettings();
            if (string.IsNullOrEmpty(Instance.PathToSaveData))
            {
                Instance.InitDefaultValues();
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
        /// Получение валидного пути, на основе настроек для сохранения файла сурса
        /// </summary>
        /// <param name="fileName">Имя файла сгенерированного сурса</param>
        /// <returns></returns>
        public static string GetPathToSaveSources(string fileName)
        {
            return Instance.PathToSaveSources + "/" + fileName;
        }
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        public static void Save()
        {
            Serializator.SaveTo<Settings>(SETTING_PATH, Instance);
        }

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
