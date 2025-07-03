#if UNITY_EDITOR
using Engine_Component.CMSSystem;
using Engine_Component.System.Serialization;
using Engine_Component.UnityIntegration.Utility;
using UnityEditor;

namespace Engine_Component.UnityIntegration.Editor
{
    public partial class CMSInspectorEditor
    {
        private CMSEntity GetDeserializedEntity()
        {
            return UnityXmlConverter.DeserializeEntityFromXml(
                SerializerUtility.GetTypeFromXmlFile(
                    AssetDatabase.GetAssetPath(_infoUtility.SelectedXmlAsset)), _infoUtility.SelectedXmlAsset) as CMSEntity;
        }
    }
}
#endif
