using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityStaticData
{
    /// <summary>
    /// Класс хранящий и управляющий настройками
    /// </summary>
    public partial class Settings
    {
        private const string DEFAULT_PATH = "Assets/DataLayer/";
        private const string SETTING_PATH = "Assets/Editor/UnityStaticData/s.set";
        private const string SOURCES_PATH = "Assets/Scripts/Data";

        /// <summary>
        /// Путь к папке сохранения данных
        /// </summary>
        public string PathToSaveData { get; set; }
        /// <summary>
        /// Путь к папке сохранения сгенерированных сурсов
        /// </summary>
        public string PathToSaveSources { get; set; }

        private void InitDefaultValues()
        {
            PathToSaveData = DEFAULT_PATH;
            PathToSaveSources = SOURCES_PATH;
        }
    }
}
