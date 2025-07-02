#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Engine_Component.UnityIntegration.Editor
{
    public class CMSInspectorWindow : CMSWindowTab
    {
        private EditorWindow _parentWindow;
        
        private TextAsset _selectedXmlAsset;
        private string _xmlText;

        private Vector2 _scrollPosition;

        public override void OnEnable(EditorWindow editorWindow)
        {
            _parentWindow = editorWindow;
        }

        public override void OnSelectionChange()
        {
            if (!IsSelectFileXml(out var file))
                return;

            _selectedXmlAsset = file;
            _xmlText = _selectedXmlAsset.text;
            _parentWindow.Repaint();
        }

        private void PanelFileXmlEditor()
        {
            EditorGUILayout.BeginHorizontal();
            {
                var fileInField = EditorGUILayout.ObjectField("XML File", _selectedXmlAsset, typeof(TextAsset), false) as TextAsset;

                if (fileInField && fileInField != _selectedXmlAsset && IsXmlFile(fileInField))
                {
                    _selectedXmlAsset = fileInField;
                    _xmlText = _selectedXmlAsset.text;
                    _parentWindow.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void PanelXmlEditor()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("XML Content", EditorStyles.boldLabel);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            {
                _xmlText = EditorGUILayout.TextArea(_xmlText, GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndScrollView();
        }

        private void PanelButtonXmlEditor()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save"))
                {
                    SaveXml();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public override void Draw()
        {
            PanelFileXmlEditor();

            if (_selectedXmlAsset)
            {
                PanelXmlEditor();
                PanelButtonXmlEditor();
            }
            else
                EditorGUILayout.HelpBox("Select an XML file to edit", MessageType.Info);
        }

        private void SaveXml()
        {
            if (!_selectedXmlAsset) return;

            var path = AssetDatabase.GetAssetPath(_selectedXmlAsset);
            File.WriteAllText(path, _xmlText);
            AssetDatabase.Refresh();

            _selectedXmlAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _selectedXmlAsset;
        }

        private bool IsSelectFileXml(out TextAsset selectFile)
        {
            var newSelection = Selection.activeObject as TextAsset;
            if (newSelection == null || newSelection == _selectedXmlAsset || !IsXmlFile(newSelection))
            {
                selectFile = null;
                return false;
            }

            selectFile = newSelection;
            return true;
        }

        private bool IsXmlFile(TextAsset asset)
        {
            if (!asset) return false;
            var path = AssetDatabase.GetAssetPath(asset);
            return path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase);
        }
    }
}
#endif
