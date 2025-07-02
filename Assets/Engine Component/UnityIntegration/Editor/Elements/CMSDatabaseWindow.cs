using Engine_Component.CMSSystem;
using Engine_Component.Utility;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace Engine_Component.UnityIntegration.Editor
{
    public class CMSDatabaseWindow : CMSWindowTab
    {
        public override void Draw()
        {
            DrawView();
            DrawEntity();
        }

        private void DrawView()
        {
            EditorGUILayout.LabelField($"Loaded Views ({CMSViewDatabase.GetAll().Count})", EditorStyles.boldLabel);

            foreach (var view in CMSViewDatabase.GetAll().OrderBy(v => v.GetType().Name))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(view.GetType().Name, EditorStyles.boldLabel);
                EditorGUILayout.ObjectField(view.gameObject, typeof(GameObject), false);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
        }

        private void DrawEntity()
        {
            EditorGUILayout.LabelField($"Loaded Entities ({CMSEntityDatabase.GetAll().Count})", EditorStyles.boldLabel);

            foreach (var entity in CMSEntityDatabase.GetAll())
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(entity.Key.Name, EditorStyles.boldLabel);

                var relativePath = PathUtility.GetRelativePath(entity.Value);

                if (GUILayout.Button("Open Path", GUILayout.Height(20)))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<Object>(relativePath);
                    if (asset != null)
                    {
                        Selection.activeObject = asset;
                        EditorGUIUtility.PingObject(asset);
                    }
                }

                EditorGUILayout.SelectableLabel(relativePath, EditorStyles.textField, GUILayout.Height(20));
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
            
        }
    }
}
#endif