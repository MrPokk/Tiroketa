#if UNITY_EDITOR
using Engine_Component.CMSSystem;
using Engine_Component.System.Serialization;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Engine_Component.UnityIntegration.Editor
{
    public partial class CmsInspectorEditor
    {
        public static void SaveXml(TextAsset xmlAsset, string xmlContent)
        {
            if (!xmlAsset) return;

            var path = AssetDatabase.GetAssetPath(xmlAsset);
            File.WriteAllText(path, xmlContent);
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        }

        public static bool IsXmlFile(TextAsset asset)
        {
            if (!asset) return false;
            var path = AssetDatabase.GetAssetPath(asset);
            return path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase);
        }

        public static bool TryGetSelectedXmlFile(out TextAsset selectedFile)
        {
            var newSelection = Selection.activeObject as TextAsset;
            if (!newSelection || !IsXmlFile(newSelection))
            {
                selectedFile = null;
                return false;
            }

            selectedFile = newSelection;
            return true;
        }

        public static CMSEntity DeserializeEntityFromXml(TextAsset xmlAsset)
        {
            if (!xmlAsset) return null;

            var path = AssetDatabase.GetAssetPath(xmlAsset);
            var type = SerializerUtility.GetTypeFromXmlFile(path);
            
            return SerializerUtility.TryDeserialize(new CMSSerializer(type, path)) as CMSEntity;
        }
        
        private CMSEntity GetDeserializedEntity()
        {
            return DeserializeEntityFromXml(_infoUtility.SelectedXmlAsset);
        }
    }
}
#endif