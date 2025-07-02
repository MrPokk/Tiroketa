using Engine_Component.Utility;
using System;
using System.IO;

namespace Engine_Component.System.Serialization
{
    public static class SerializerUtility
    {
        public static string TrySerialize(ISerializerProvider providerSerializer, Type typeObject, string pathBase)
        {
            if (typeObject.IsAbstract)
                throw new ArgumentException($"ERROR: {typeObject} is Abstract");

            PathUtility.ValidatePath(ref pathBase);

            var valueObject = Activator.CreateInstance(typeObject);
            var serializer = providerSerializer.Serialization(valueObject);

            var finalPath = $"{pathBase}{typeObject.Name}.xml";
            using var fileStream = new FileStream(finalPath, FileMode.Create);
            serializer.Serialize(fileStream, valueObject);

            return finalPath;
        }

        public static T TryDeserialize<T>(ISerializerProvider providerSerializer, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var deserializedObject = providerSerializer.Deserialize<T>(filePath);

            if (deserializedObject == null)
                throw new InvalidOperationException($"Failed to deserialize object of type from {filePath}");

            return deserializedObject;
        }
    }
}
