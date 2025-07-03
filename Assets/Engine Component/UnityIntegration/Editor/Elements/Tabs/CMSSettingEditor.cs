#if UNITY_EDITOR
using Engine_Component.Utility;
using UnityEditor;
using UnityEngine;

namespace Engine_Component.UnityIntegration.Editor
{
    public class CMSSettingEditor : CMSEditorTab
    {
        public override void Draw()
        {
            EditorGUILayout.LabelField("CMS Settings", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Create const path", GUILayout.Height(25)))
                PathUtility.GenerationConstPath();
        }
    }
}
#endif