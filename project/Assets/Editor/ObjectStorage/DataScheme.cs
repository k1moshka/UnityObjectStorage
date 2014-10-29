﻿using System;
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
        /// Происходит когда изменятеся одно из свойств схемы данных
        /// </summary>
        public event Action OnChanged;

        // <fieldName, typeName 
        /// <summary>
        /// Все поля схемы, <имя поля, имя типа поля>
        /// </summary>
        public Dictionary<string, TypeDescriptor> Fields { get; set; }
        /// <summary>
        /// Тип генерируемых сущностей
        /// </summary>
        public DataType DataType { get; set; }
        /// <summary>
        /// Место хранения данных схемы
        /// </summary>
        public StorageType StorageType { get; set; }
        /// <summary>
        /// Название генерируемого типа для схемы
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Сгенерирована ли сущность для схемы
        /// </summary>
        public bool IsGenerated { get { return System.IO.File.Exists(SourceGenerator.GetSourcePath(TypeName)); } }

        /// <summary>
        /// Создание нового экземпляра
        /// </summary>
        public DataScheme()
        {
            Fields = new Dictionary<string, TypeDescriptor>();
        }
        /// <summary>
        /// Добавление нового поля в схеиу данных
        /// </summary>
        /// <param name="name">Имя поля</param>
        /// <param name="field">Описатель типа поля</param>
        public void AddField(string name, TypeDescriptor field)
        {
            var needRaise = false;
            if (!Fields.ContainsKey(name))
                needRaise = true;

            Fields[name] = field;

            if (needRaise) raiseOnChanged();
        }
        /// <summary>
        /// Удаление поля из схемы данных
        /// </summary>
        /// <param name="name">Имя поля</param>
        public void RemoveField(string name)
        {
            if (Fields.ContainsKey(name))
            {
                Fields.Remove(name);
                raiseOnChanged();
            }
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

            foreach (var fieldName in Fields.Keys)
            {
                if (regex.IsMatch(fieldName) || fieldName == TypeName)
                {
                    errorMessage = fieldName + " is invalid.";
                    return false;
                }
            }

            return true;
        }

        private void raiseOnChanged()
        {
            if (OnChanged != null)
                OnChanged();
        }
    }
}