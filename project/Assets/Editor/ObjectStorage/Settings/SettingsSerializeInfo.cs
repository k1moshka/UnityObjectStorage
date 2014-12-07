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
        private bool canWrite = false;

        private const string DEFAULT_PATH = "Assets/DataLayer/";
        private const string SETTING_PATH = "Assets/Editor/UnityStaticData/s.set";
        private const string SOURCES_PATH = "Assets/Scripts/Data";
        private const string RESOURCES_FILE_NAME = "dataLayerResources";
        private const string RESOURCES_PATH = "Assets/Resources/";

        /// <summary>
        /// Путь к папке сохранения данных
        /// </summary>
        public string PathToSaveData { get; set; }
        /// <summary>
        /// Путь к папке сохранения сгенерированных сурсов
        /// </summary>
        public string PathToSaveSources { get; set; }
        /// <summary>
        /// Имя файла ресурсов репозитория
        /// </summary>
        public string ResourcesFileName { get; set; }
        /// <summary>
        /// Типы которые не сериализуются в хранилище, а загружаются из ресурсов проекта
        /// </summary>
        public string[] LinkedTypes { get; set; }

        private List<TypeDescriptor> types = new List<TypeDescriptor>();
        // Записывается только один раз на загрузке настроек, при дессериализации ли создании новых настроек
        /// <summary>
        /// Все зарегестрированные типы для полей
        /// </summary>
        public TypeDescriptor[] AvailableTypes { get { return types.ToArray(); } set { if (canWrite) { types.Clear(); types.AddRange(value); canWrite = false; } } }

        private void InitDefaultValues()
        {
            PathToSaveData = DEFAULT_PATH;
            PathToSaveSources = SOURCES_PATH;
            ResourcesFileName = RESOURCES_FILE_NAME;
            types = new List<TypeDescriptor>(
                new TypeDescriptor[] { 
                    new TypeDescriptor() { TypeName = "System.Int32", ToStringValue = "int", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderInt" }, 
                    new TypeDescriptor() { TypeName = "System.String",  RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderString" }, 
                    new TypeDescriptor() { TypeName = "System.Single", ToStringValue = "float", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderFloat" }, 
                    new TypeDescriptor() { TypeName = "System.Double",  RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderFloat" }, 
                    new TypeDescriptor() { TypeName = "System.Decimal", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderDecimal" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Sprite", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderSprite" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Object", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderUnityObject" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.AudioClip", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderAudio" },
                }
            );
            LinkedTypes = new string[]
            {
                "UnityEngine.Sprite",
                "UnityEngine.Object",
                "UnityEngine.AudioClip",
            };
        }

        #region ctors
        public Settings()
        {

        }

        public Settings(bool manual)
        {
            InitDefaultValues();
        }
        #endregion
    }
}
