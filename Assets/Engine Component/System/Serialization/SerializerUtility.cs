using Engine_Component.Utility;
using System;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace Engine_Component.System.Serialization
{
    public static class SerializerUtility
    {
        public static string TrySerialize(ISerializerProvider providerSerializer, Type typeObject, string pathBase)
        {
            try
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
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Serialization entity: {ex.Message}");
            }
        }

        public static T TryDeserialize<T>(ISerializerProvider providerSerializer, string filePath)
        {
            try
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
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Deserialize entity: {ex.Message}");
            }
        }
    }
}
