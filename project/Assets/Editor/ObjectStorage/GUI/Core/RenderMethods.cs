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
        // TODO: написать гуи для полей decimal, quaternion, даты, времени

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
    }
}
