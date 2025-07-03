#if UNITY_EDITOR
using UnityEditor;

namespace Engine_Component.UnityIntegration.Editor
{
    public partial class CmsInspectorEditor
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
            if (!TryGetSelectedXmlFile(out var selectedFile))
                return;
            
            _infoUtility.SelectedXmlAsset = selectedFile;
            _infoUtility.XMLText = _infoUtility.SelectedXmlAsset.text;
            _infoUtility.DeserializedEntity = GetDeserializedEntity();
        }
    }
}
#endif
