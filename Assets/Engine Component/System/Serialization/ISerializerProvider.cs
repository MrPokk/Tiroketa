using System;
using System.IO;
using System.Xml.Serialization;

namespace Engine_Component.System.Serialization
{
    public interface ISerializerProvider
    {
        public XmlSerializer Serialization(object value);
        public T Deserialize<T>(string filePath)
        {
            var serializer = Serialization(typeof(T));

            using var fileStream = new FileStream(filePath, FileMode.Open);
            return (T)serializer.Deserialize(fileStream);
        }
    }
}
