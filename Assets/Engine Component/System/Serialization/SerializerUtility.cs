using Engine_Component.Utility.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Engine_Component.System.Serialization
{
    public static class SerializerUtility
    {

        public static string TrySerialize(object objectValue, string fullPath)
        {
            ValidationPath(fullPath);

            using var fileStream = new FileStream(fullPath, FileMode.Open);

            GetXmlSerializer(objectValue).Serialize(fileStream, objectValue);
            return null;
        }

        public static string TrySerialize(Type typeSerializer, string fullPath)
        {
            try
            {
                if (typeSerializer.IsAbstract)
                    throw new ArgumentException($"ERROR: {typeSerializer} is Abstract");

                ValidationPath(fullPath);

                using var fileStream = new FileStream(fullPath, FileMode.Create);

                GetXmlSerializer(typeSerializer, out var instanceObject)?.Serialize(fileStream, instanceObject);

                return fullPath;
            }
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Serialization entity: {ex.Message}");
            }
        }

        public static string TrySerialize(ISerializerProvider serializerProvider)
        {
            return serializerProvider.Serialization();
        }

        public static object TryDeserialize(Type typeSerializer, string fullPath)
        {
            try
            {
                ValidationPath(fullPath);

                var serializer = GetXmlSerializer(typeSerializer, out var instanceObject);

                using var fileStream = new FileStream(fullPath, FileMode.Open);

                return serializer.Deserialize(fileStream);
            }
            catch (SerializationException ex)
            {
                throw new SerializationException($"Failed to Deserialize entity: {ex.Message}");
            }
        }

        public static object TryDeserialize(ISerializerProvider serializerProvider)
        {
            return serializerProvider.Deserialize();
        }

        public static T TryDeserialize<T>(string fullPath) where T : class, new()
        {
            return TryDeserialize(typeof(T), fullPath) as T;
        }

        public static T TryDeserialize<T>(ISerializerProvider serializerProvider) where T : class, new()
        {
            return TryDeserialize(serializerProvider) as T;
        }

        public static Type GetTypeFromXmlFile(string xmlFilePath, Type typeComparison = null)
        {
            ValidationPath(xmlFilePath);

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

        private static XmlSerializer GetXmlSerializer(Type typeSerializer, out object instanceObject)
        {
            XmlSerializer serializer;
            
            instanceObject = Activator.CreateInstance(typeSerializer);
            if (instanceObject is IXmlIncludeExtraType includeExtraType)
                serializer = new XmlSerializer(typeSerializer, includeExtraType.GetExtraType());

            else
                serializer = new XmlSerializer(typeSerializer);

            return serializer;
        }

        private static XmlSerializer GetXmlSerializer(object objectValue)
        {
            XmlSerializer serializer;
            if (objectValue is IXmlIncludeExtraType includeExtraType)
                serializer = new XmlSerializer(objectValue.GetType(), includeExtraType.GetExtraType());
            else
                serializer = new XmlSerializer(objectValue.GetType());

            return serializer;
        }

        private static void ValidationPath(string fullPath)
        {
            if (fullPath == null || !File.Exists(fullPath))
                throw new AggregateException($"ERROR: path not valid: {fullPath}");
        }
    }
}
