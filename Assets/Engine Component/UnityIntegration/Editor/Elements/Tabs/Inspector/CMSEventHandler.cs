#if UNITY_EDITOR
using Engine_Component.UnityIntegration.Utility;
using UnityEditor;

namespace Engine_Component.UnityIntegration.Editor
{
    public partial class CMSInspectorEditor
    {
        public override void OnEnable(EditorWindow editorWindow)
        {
            _parentWindow = editorWindow;
        }

        public override void OnSelectionChange()
        {
            UpdateInfo();
            _parentWindow.Repaint();
        }

        private void UpdateInfo()
        {
            if (UnityXmlConverter.TryGetSelectedXmlFile(out var selectedFile))
            {
                _infoUtility.SelectedXmlAsset = selectedFile;
                _infoUtility.XMLText = _infoUtility.SelectedXmlAsset.text;
                _infoUtility.DeserializedEntity = GetDeserializedEntity();
            }
            else
            {
                _infoUtility.SelectedXmlAsset = null;
                _infoUtility.XMLText = null;
                _infoUtility.DeserializedEntity = null;
            }
        
        }
    }
}
#endif
