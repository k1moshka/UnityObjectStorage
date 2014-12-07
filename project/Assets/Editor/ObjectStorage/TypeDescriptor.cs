using System;
using System.Linq;
using UnityEngine;

namespace UnityStaticData
{
    /// <summary>
    /// Описатель типа
    /// </summary>
    [Serializable]
    public struct TypeDescriptor
    {
        /// <summary>
        /// Полное название типа, включая пространство имен
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Значение которое отображается в комбобоксах, если null то берется 
        /// </summary>
        public string ToStringValue { get; set; }

        /// <summary>
        /// Получение описываемого типа как Type
        /// </summary>
        /// <returns></returns>
        public new Type GetType()
        {
            return Type.GetType(TypeName);
        }

        public override string ToString()
        {
            return ToStringValue ?? TypeName.Split('.').Last();
        }

        #region rendering field
        private RenderCustomField renderMethod;

        private string renderMethodType;
        /// <summary>
        /// Тип который хранит метод, для отрисовки. Сигнатура должна совпадать с RenderCustomField делегатом
        /// </summary>
        public string RenderMethodType
        {
            get
            {
                return renderMethodType;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Type name that provided method is invalid.");

                renderMethodType = value;

                loadMethod();
            }
        }

        private string renderMethodName;
        /// <summary>
        /// Полное имя включая нэймспэйс объекта в котором хранится метод для отрисовывания поля для типа. Метод обязательно должен быть статическим.
        /// </summary>
        public string RenderMethodName
        {
            get
            {
                return renderMethodName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Name of method is invalid.");

                renderMethodName = value;

                loadMethod();
            }
        }
        /// <summary>
        /// Отрисовка поля
        /// </summary>
        /// <param name="value">Текущее значение поля</param>
        /// <returns></returns>
        public object RenderField(string label, object value)
        {
            if (renderMethod != null)
                return renderMethod(label, value);
            return null;
        }

        private void loadMethod()
        {
            if (string.IsNullOrEmpty(renderMethodName) || string.IsNullOrEmpty(renderMethodType))
                return;

            var type = System.Type.GetType(renderMethodType);

            if (type == null)
                throw new Exception("Type " + renderMethodType + " couldnt load from assembly");

            var method = Delegate.CreateDelegate(typeof(RenderCustomField), type.GetMethod(RenderMethodName));

            if (method == null)
                throw new Exception("Render method couldnt load from assembly");

            renderMethod = method as RenderCustomField;
        }
        #endregion
    }
}