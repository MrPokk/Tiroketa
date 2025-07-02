using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Engine_Component.System.Serialization
{
    public static class SerializerUtility
    {
        public static string TrySerialize(ISerializerProvider serializerProvider)
        {
            try
            {
                var typeObject = serializerProvider.GetObjectType();
                var fullPath = serializerProvider.GetFullPath();

                if (typeObject.IsAbstract)
                    throw new ArgumentException($"ERROR: {typeObject} is Abstract");

                if (fullPath == null || !File.Exists(fullPath))
                    throw new AggregateException("ERROR: path not valid");

                var serializer = GetSerializer(serializerProvider, out var instanceObject);

                using var fileStream = new FileStream(fullPath, FileMode.Create);
                serializer?.Serialize(fileStream, instanceObject);

                return fullPath;
            }
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Serialization entity: {ex.Message}");
            }
        }

        public static T TryDeserialize<T>(ISerializerProvider serializerProvider)
        {
            try
            {
                var filePath = serializerProvider.GetFullPath();

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                var serializer = GetSerializer(serializerProvider, out var instanceObject);

                if (instanceObject.GetType() != typeof(T))
                    throw new TypeAccessException($"ERROR: object {instanceObject} in not type T");

                using var fileStream = new FileStream(filePath, FileMode.Open);
                return (T)serializer.Deserialize(fileStream);
            }
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Deserialize entity: {ex.Message}");
            }
        }

        private static XmlSerializer GetSerializer(ISerializerProvider serializerProvider, out object instanceObject)
        {
            var valueObject = Activator.CreateInstance(serializerProvider.GetObjectType());
            instanceObject = valueObject;
            return serializerProvider.Serialization(ref valueObject);
        }
    }
}
