using System;
using System.IO;
using System.Linq;

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
        /// Регистрация нового поддерживаемого схемами данных типа
        /// </summary>
        /// <param name="descr">Описатель нового типа</param>
        public static void RegisterNewType(TypeDescriptor descr)
        {
            Instance.types.Add(descr);
        }
        /// <summary>
        /// Получение всех зарегистрированных типов в виде массива строк
        /// </summary>
        /// <returns></returns>
        public static string[] GetRegisteredTypes()
        {
            var result = new string[Instance.types.Count]; 
            for (var i = 0; i < Instance.types.Count; i++)
            {
                result[i] = Instance.types[i].ToString();
            } 
            return result;
        }
        /// <summary>
        /// Получение нужного дескриптора, по его строковому представлению
        /// </summary>
        /// <param name="toStringValue">Строковое представление</param>
        /// <returns></returns>
        public static TypeDescriptor GetDescriptor(string toStringValue)
        {
            return Instance.types.First(t => t.ToStringValue == toStringValue || t.TypeName.Contains(toStringValue));
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
