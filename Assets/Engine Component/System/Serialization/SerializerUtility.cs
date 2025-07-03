using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using FileNotFoundException = System.IO.FileNotFoundException;

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

        public static object TryDeserialize(ISerializerProvider serializerProvider)
        {
            try
            {
                var filePath = serializerProvider.GetFullPath();

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                var serializer = GetSerializer(serializerProvider, out var instanceObject);

                if (instanceObject.GetType() != serializerProvider.GetObjectType())
                    throw new TypeAccessException($"ERROR: object {instanceObject} in not type Params");
                
                using var fileStream = new FileStream(filePath, FileMode.Open);

                var objectDeserialize = serializer.Deserialize(fileStream);
                
                return objectDeserialize;
            }
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Deserialize entity: {ex.Message}");
            }
        }

        public static T TryDeserialize<T>(ISerializerProvider serializerProvider) where T : class
        {
            return TryDeserialize(serializerProvider) as T;
        }


        public static Type GetTypeFromXmlFile(string xmlFilePath, Type typeComparison = null)
        {
            if (!File.Exists(xmlFilePath))
                throw new FileNotFoundException($"XML file not found at path: {xmlFilePath}", xmlFilePath);

            try
            {
                var settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true,
                    DtdProcessing = DtdProcessing.Ignore
                };

                using var reader = XmlReader.Create(xmlFilePath, settings);

                reader.MoveToContent();
                var rootElementName = reader.LocalName;

                var foundType = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == rootElementName &&
                                         (typeComparison == null || typeComparison.IsAssignableFrom(t)));

                if (foundType != null)
                    return foundType;

                var errorMessage = typeComparison != null
                    ? $"Type '{rootElementName}' not found or not derived from {typeComparison.Name}"
                    : $"Type '{rootElementName}' not found in loaded assemblies";

                throw new InvalidOperationException(errorMessage);

            }
            catch (XmlException ex)
            {
                throw new XmlException($"Failed to parse XML file: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error while reading XML file: {ex.Message}", ex);
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
