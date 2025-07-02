using Engine_Component.CMSSystem;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Engine_Component.System.Serialization
{
    public sealed class CMSSerializerToDatabase : ISerializerProvider
    {
        public XmlSerializer Serialization(object value)
        {
            if (value is not CMSEntity cmsEntity)
                throw new ArgumentException("ERROR: valueObject is not CMSEntity");

            if (TryGetComponentTypes(cmsEntity, out var allSerialize))
                return new XmlSerializer(value.GetType(), allSerialize);
            else
                return new XmlSerializer(value.GetType());
        }
        public T Deserialize<T>(string filePath)
        {
            var serializer = Serialization(typeof(T));

            using var fileStream = new FileStream(filePath, FileMode.Open);
            return (T)serializer.Deserialize(fileStream);
        }

        private bool TryGetComponentTypes(CMSEntity entity, out Type[] allSerialize)
        {
            var allSerializableComponent = entity.GetAllComponent().Keys.Where(component =>
                component.IsDefined(typeof(SerializableAttribute), false)).ToArray();

            allSerialize = allSerializableComponent;
            return allSerializableComponent.Any();
        }
    }
}
