#if UNITY_EDITOR
using Engine_Component.CMSSystem;
using UnityEngine;

namespace Engine_Component.UnityIntegration.Editor
{
    public class InspectorInfoUtility
    {
        public TextAsset SelectedXmlAsset;
        public string XMLText;

        public CMSEntity DeserializedEntity;
    }
}
#endif
