using System;
using System.Xml.Serialization;

namespace Engine_Component.System.Serialization
{
    public interface ISerializerProvider
    {
        Type GetObjectType();
        string GetFullPath();
        XmlSerializer Serialization(ref object value);
    }
}
