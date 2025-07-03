using Engine_Component.CMSSystem;
using System;
using System.Xml.Serialization;

namespace Engine_Component.System.Serialization
{
    public sealed class CMSSerializer : ISerializerProvider
    {
        private readonly Type _typeObject;
        private readonly string _fullPath;

        public CMSSerializer(Type typeObject, string fullPath)
        {
            _typeObject = typeObject;
            _fullPath = fullPath;
        }

        public Type GetObjectType() => _typeObject;
        public string GetFullPath() => _fullPath;

        public XmlSerializer Serialization(ref object valueObject)
        {
            if (valueObject is not CMSEntity cmsEntity)
                throw new ArgumentException($"ERROR: {valueObject} is not CMSEntity his type: {valueObject.GetType()}");

            return cmsEntity.TryGetSerializeComponentTypes(out var allSerialize)
                ? new XmlSerializer(valueObject.GetType(), allSerialize)
                : new XmlSerializer(valueObject.GetType());
        }
    }
}
