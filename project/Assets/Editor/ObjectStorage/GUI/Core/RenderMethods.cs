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
        /// Отрисовка поля для префаба
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderUnityObject(string label, object value)
        {
            var obj = value as LinkedObject;

            obj.Object = EditorGUILayout.ObjectField(label, obj.Object, typeof(UnityEngine.Object), false);
            obj.Link = AssetDatabase.GetAssetPath(obj.Object);

            return obj;
        }
        /// <summary>
        /// Отрисовка поля для спрайта
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderSprite(string label, object value)
        {
            var obj = value as LinkedObject;

            obj.Object = EditorGUILayout.ObjectField(label, obj.Object, typeof(Sprite), false);
            obj.Link = AssetDatabase.GetAssetPath(obj.Object);

            return obj;
        }
        /// <summary>
        /// Отрисовка поля для аудиоклипа
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderAudio(string label, object value)
        {
            var obj = value as LinkedObject;

            obj.Object = EditorGUILayout.ObjectField(label, obj.Object, typeof(AudioClip), false);
            obj.Link = AssetDatabase.GetAssetPath(obj.Object);

            return obj;
        }
        /// <summary>
        /// Отрисовка decimal
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object RenderDecimal(string label, object value)
        {
            return Convert.ToDecimal(EditorGUILayout.FloatField(label, Convert.ToSingle((decimal)value)));
        }
    }
}
