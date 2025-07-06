#if UNITY_EDITOR
using BitterCMS.CMSSystem;
using BitterCMS.System.Serialization;
using BitterCMS.Utility;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitterCMS.UnityIntegration.Editor
{
    public class CMSDatabaseEditor : CMSEditorTab
    {
        public override void Draw()
        {
            DrawView();
            DrawEntity();
        }

        private void DrawView()
        {
            var allView = CMSViewDatabase.GetAll();
            
            EditorGUILayout.LabelField($"Loaded Views ({allView.Count})", EditorStyles.boldLabel);
            if (!allView.Any())
                return;
            
            foreach (var view in allView.OrderBy(v => v.GetType().Name))
            {
                if (!view)
                    continue;
                
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(view.GetType().Name, EditorStyles.boldLabel);
                EditorGUILayout.ObjectField(view.gameObject, typeof(GameObject), false);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
        }

        private void DrawEntity()
        {
            var allEntity = CMSEntityDatabase.GetAll();
            var isRecreate = false;   
            
            EditorGUILayout.LabelField($"Loaded Entities ({allEntity.Count})", EditorStyles.boldLabel);

            foreach (var entity in allEntity)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(entity.Key.Name, EditorStyles.boldLabel);

                var relativePath = PathUtility.GetRelativePath(entity.Value);

                if (GUILayout.Button("Create", GUILayout.Height(20)))
                {
                    SerializerUtility.TrySerialize(entity.Key, entity.Value);
                    isRecreate = true;
                }

                EditorGUILayout.SelectableLabel(relativePath, EditorStyles.textField, GUILayout.Height(20));
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }

            if (isRecreate)
                AssetDatabase.Refresh();
        }
    }
}
#endif