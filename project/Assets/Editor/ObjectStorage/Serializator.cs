using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace UnityStaticData
{
    /// <summary>
    /// Сериализатор объектов, который сам знает в каком формате нужно сохранить объект
    /// </summary>
    public static class Serializator
    {
        /// <summary>
        /// Формат сохранения объектов
        /// </summary>
        public enum SerializationType
        {
            /// <summary>
            /// Бинарный
            /// </summary>
            Binary,
            /// <summary>
            /// ХМЛ
            /// </summary>
            XML,
            /// <summary>
            /// ДЖСОН
            /// </summary>
            //JSON,
        }
        /// <summary>
        /// Текущий тип сохранения данных
        /// </summary>
        public static SerializationType type = SerializationType.Binary;

        private static ISerializationProvider _serializator
        {
            get
            {
                switch (type)
                {
                    case SerializationType.Binary:
                        return binSer;
                    case SerializationType.XML:
                        return xmlSer;
                    //case SerializationType.JSON:
                    //    return jsonSer;
                    default:
                        break;
                }
                return null;
            }
        }
        private static ISerializationProvider xmlSer, jsonSer, binSer;

        static Serializator()
        {
            xmlSer = new XMLSerializator();
            jsonSer = new JSONSerializator();
            binSer = new BinarySerializator();
        }

        /// <summary>
        /// Сохранить экземпляр, на основе настроек
        /// </summary>
        /// <typeparam name="T">Тип экземплра, обязательно помеченный как Serializable</typeparam>
        /// <param name="path">Путь для сохранения файла</param>
        /// <param name="obj">Сохраняемый экземпляр</param>
        public static void SaveTo<T>(string path, T obj)
        {
            if (File.Exists(path))
                File.Delete(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(Directory.GetParent(path).ToString());

            var f = File.Create(path);
            f.Close();
            f.Dispose();

            _serializator.Serialize<T>(path, obj);
        }
        /// <summary>
        /// Загрузка объекта из постоянного хранилища
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="path">Путь до файла с объектом</param>
        /// <returns></returns>
        public static T LoadFrom<T>(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File: " + path + " not found");

            return _serializator.Deserialize<T>(path);
        }

        #region current serializations
        /// <summary>
        /// Интерфейс сериализаторов
        /// </summary>
        private interface ISerializationProvider
        {
            void Serialize<T>(string path, T obj);
            T Deserialize<T>(string path);
        }
        /// <summary>
        /// Бинарный сериализатор
        /// </summary>
        private class BinarySerializator : ISerializationProvider
        {

            public void Serialize<T>(string path, T obj)
            {
                using (var stream = File.OpenWrite(path))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                }
            }

            public T Deserialize<T>(string path)
            {
                T result;
                using (var stream = File.OpenWrite(path))
                {
                    var formatter = new BinaryFormatter();
                    result = (T)formatter.Deserialize(stream);
                }
                return result;
            }
        }
        /// <summary>
        /// ХМЛ сериализатор
        /// </summary>
        private class XMLSerializator : ISerializationProvider
        {

            public void Serialize<T>(string path, T obj)
            {
                using (var stream = File.OpenWrite(path))
                {
                    var formatter = new XmlSerializer(typeof(T));
                    formatter.Serialize(stream, obj);
                }
            }


            public T Deserialize<T>(string path)
            {
                T result;
                using (var stream = File.OpenWrite(path))
                {
                    var formatter = new XmlSerializer(typeof(T));
                    result = (T)formatter.Deserialize(stream);
                }
                return result;
            }
        }
        /// <summary>
        /// ДЖСОН сериализатор
        /// </summary>
        private class JSONSerializator : ISerializationProvider
        {

            public void Serialize<T>(string path, T obj)
            {
                throw new NotImplementedException();
            }


            public T Deserialize<T>(string path)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}