using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityStaticData
{
    /// <summary>
    /// Схема данных
    /// </summary>
    [Serializable]
    public class DataScheme
    {
        /// <summary>
        /// Происходит когда изменятся одно из свойств схемы данных. Передается имя схемы
        /// </summary>
        [field: NonSerialized]
        public event Action<string> OnFieldsChanged;
        /// <summary>
        /// Происходит когда схема данных переименовывается. Передается (старое название, новое название)
        /// </summary>
        [field: NonSerialized]
        public event Action<string, string> OnRenameScheme;

        // <fieldKey, field>
        /// <summary>
        /// Все поля схемы, (fieldKey, field)
        /// </summary>
        public Dictionary<string, Field> Fields { get; set; }
        /// <summary>
        /// Тип генерируемых сущностей
        /// </summary>
        public DataType DataType { get; set; }
        /// <summary>
        /// Место хранения данных схемы
        /// </summary>
        public StorageType StorageType { get; set; }

        private string typeName;
        /// <summary>
        /// Название генерируемого типа для схемы
        /// </summary>
        public string TypeName { get { return typeName; } set { var lastName = typeName; typeName = value; if (lastName != value) raiseRename(lastName); } }
        /// <summary>
        /// Сгенерирована ли сущность для схемы
        /// </summary>
        public bool IsGenerated { get { return System.IO.File.Exists(EntitySourceGenerator.GetSourcePath(TypeName)); } }
        /// <summary>
        /// Насоелуемый тип, по дефолту EntityBase
        /// </summary>
        public string InheritanceType { get; set; }

        /// <summary>
        /// Создание нового экземпляра
        /// </summary>
        public DataScheme()
        {
            Fields = new Dictionary<string, Field>();
            InheritanceType = "EntityBase";
        }
        /// <summary>
        /// Добавление нового поля в схему данных. Возвращает ключ для нового поля.
        /// </summary>
        /// <param name="name">Имя поля</param>
        /// <param name="field">Описатель типа поля</param>
        public string AddField(Field field)
        {
            var key = KeyGenerator.GenerateStringKey();

            Fields[key] = field;

            RaiseChanged();

            return key;
        }
        /// <summary>
        /// Удаление поля из схемы данных
        /// </summary>
        /// <param name="name">Имя поля</param>
        public void RemoveField(string fieldKey)
        {
            if (Fields.ContainsKey(fieldKey))
            {
                Fields.Remove(fieldKey);
                RaiseChanged();
            }
        }
        /// <summary>
        /// Удаление поля из схемы по имени поля
        /// </summary>
        /// <param name="fieldName">Имя поля в схеме данных</param>
        public void RemoveFieldByName(string fieldName)
        {
            var removingField = Fields.FirstOrDefault(f => f.Value.Name == fieldName);

            if (removingField.Value != null && removingField.Key != null)
                Fields.Remove(removingField.Key);
        }
        /// <summary>
        /// Валидация экземпляра
        /// </summary>
        /// <returns></returns>
        public bool IsValid(out string errorMessage)
        {
            errorMessage = null;
            var regex = new Regex(@"^\d|[\s,.:;'" + '"' + @"\[\]\(\)\*\&\^\$\#\!\~\<\>]");

            if ((!string.IsNullOrEmpty(TypeName) && !regex.IsMatch(TypeName)) == false)
            {
                errorMessage = "Scheme name is invalid.";
                return false;
            }

            foreach (var field in Fields.Values)
            {
                if (regex.IsMatch(field.Name) || field.Name == TypeName)
                {
                    errorMessage = "Field " + field + " is invalid.";
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Очистка всех обрабочиков событий для схемы
        /// </summary>
        public void CleanUpHandlers()
        {
            OnFieldsChanged = null;
        }

        #region event raisers
        public void RaiseChanged()
        {
            if (OnFieldsChanged != null)
                OnFieldsChanged(TypeName);
        }

        private void raiseRename(string lastName)
        {
            if (OnRenameScheme != null)
                OnRenameScheme(lastName, typeName);
        }
        #endregion
    }
}