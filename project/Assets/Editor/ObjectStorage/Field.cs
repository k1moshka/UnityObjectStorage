using System;
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

            // TODO: разобраться с загрузкой типов из UnityEngine
            Debug.Log(string.Format("typeName:{0}", new object[] { typeDescr.TypeName }));
            var type = this.Type.GetType();
            
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
        public object RenderField(string fieldName)
        {
            return Value = Type.RenderField(fieldName, Value);
        }
    }
}
