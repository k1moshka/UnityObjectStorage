using System;
using UnityEngine;
using UnityEditor;

namespace UnityStaticData
{
    /// <summary>
    /// Класс хранящий методы для отрисовки дефолтных полей
    /// </summary>
    public static class RenderMethods
    {
        /// <summary>
        /// Отрисовка целочисленного поля
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderInt(string label, object value)
        {
            return EditorGUILayout.IntField(label, (int)value);
        }
        /// <summary>
        /// Отрисовка строкового поля
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderString(string label, object value)
        {
            return EditorGUILayout.TextField(label, (string)value);
        }
        /// <summary>
        /// Отрисовка для float
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderFloat(string label, object value)
        {
            return EditorGUILayout.FloatField(label, (float)value);
        }
        /// <summary>
        /// Отрисовка поля для Vector2
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderVector2(string label, object value)
        {
            return EditorGUILayout.Vector2Field(label, (Vector2)value);
        }
        /// <summary>
        /// Отрисовка поля для Vector3
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderVector3(string label, object value)
        {
            return EditorGUILayout.Vector3Field(label, (Vector3)value);
        }
        /// <summary>
        /// Отрисовка поля для Vector4
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderVector4(string label, object value)
        {
            return EditorGUILayout.Vector4Field(label, (Vector4)value);
        }
        /// <summary>
        /// Отрисовка поля для спрайта
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderUnityObject(string label, object value)
        {
            return EditorGUILayout.ObjectField(label, (Sprite)value, typeof(Sprite), false);
        }
        /// <summary>
        /// Отрисовка поля для префаба
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderSprite(string label, object value)
        {
            return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, typeof(UnityEngine.Object), false);
        }
        /// <summary>
        /// Отрисовка кватерниона
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderQuaternion(string label, object value)
        {
            var vector = (Vector4)EditorGUILayout.Vector4Field(label, QuaternionToVector4((Quaternion)value));
            return new Quaternion(vector.x, vector.y, vector.z, vector.w);
        }
        /// <summary>
        /// Отрисовка decimal
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderDecimal(string label, object value)
        {
            return (decimal)EditorGUILayout.FloatField(label, (float)value);
        }
        // TODO: ADVANCED: ADD DATETIME FIELD
        /// <summary>
        /// Отрисовка времени и даты
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderDateTime(string label, object value)
        {
            return null;
        }

        private static Vector4 QuaternionToVector4(Quaternion rot) 
        {
			return new Vector4(rot.x, rot.y, rot.z, rot.w);
		}
    }
}
