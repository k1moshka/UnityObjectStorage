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

        /// <summary>
        /// Путь к папке сохранения данных
        /// </summary>
        public string PathToSaveData { get; set; }
        /// <summary>
        /// Путь к папке сохранения сгенерированных сурсов
        /// </summary>
        public string PathToSaveSources { get; set; }

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
            types = new List<TypeDescriptor>(
                new TypeDescriptor[] { 
                    new TypeDescriptor() { TypeName = "System.Int32", ToStringValue = "int", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderInt" }, 
                    new TypeDescriptor() { TypeName = "System.String",  RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderString" }, 
                    new TypeDescriptor() { TypeName = "System.Single", ToStringValue = "float", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderFloat" }, 
                    new TypeDescriptor() { TypeName = "System.Double",  RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderFloat" }, 
                    new TypeDescriptor() { TypeName = "System.Decimal", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderDecimal" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Vector2", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderVector2" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Vector3", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderVector3" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Vector4", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderVector4" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Quaternion", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderQuaternion" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.Sprite", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderSprite" }, 
                    new TypeDescriptor() { TypeName = "UnityEngine.UnityObject", RenderMethodType = "UnityStaticData.RenderMethods", RenderMethodName = "RenderUnityObject" }, 
                }
            );
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
