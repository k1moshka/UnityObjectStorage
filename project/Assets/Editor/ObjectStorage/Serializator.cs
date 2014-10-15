using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public static class Serializator
{
    public enum SerializationType
	{
        Binary,
        XML,
        //JSON,
	}
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

    public static void SaveTo<T>(string path, T obj)
    {
        _serializator.Serialize<T>(path, obj);
    }

    public static T LoadFrom<T>(string path)
    {
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
