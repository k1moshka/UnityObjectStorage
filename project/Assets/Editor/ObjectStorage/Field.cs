﻿using System;
using System.Reflection;
using UnityEngine;

namespace UnityStaticData
{
    /// <summary>
    /// Описатель поля
    /// </summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// Имя поля в схеме данных
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение поля
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Тип поля
        /// </summary>
        public TypeDescriptor Type { get; set; }

        // для десериализации
        public Field()
        {
            
        }
        // для создания нового инстанса
        public Field(TypeDescriptor typeDescr)
        {
            this.Type = typeDescr;

            var type = this.Type.GetType();

            if (type == null)
            {
                var assembly = Assembly.GetAssembly(typeof(Vector2));
                type = assembly.GetType(typeDescr.TypeName);
            }

            if (type.IsClass)
                Value = null;
            else
                Value = type.InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
        }

        /// <summary>
        /// Отрисовка поля
        /// </summary>
        /// <param name="value">Текущее значение поля</param>
        /// <returns></returns>
        public object RenderField()
        {
            return Value = Type.RenderField(Name, Value);
        }
    }
}
