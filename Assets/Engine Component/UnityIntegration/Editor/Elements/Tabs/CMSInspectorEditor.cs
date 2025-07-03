#if UNITY_EDITOR
using Engine_Component.System.Serialization;
using Engine_Component.UnityIntegration.Utility;
using UnityEditor;
using UnityEngine;

namespace Engine_Component.UnityIntegration.Editor
{
    public partial class CMSInspectorEditor : CMSEditorTab
    {
        private EditorWindow _parentWindow;
        private readonly InspectorInfoUtility _infoUtility = new InspectorInfoUtility();
        private bool _showXmlView = false;

        public override void Draw()
        {
            PanelFileXmlEditor();

            if (_infoUtility.SelectedXmlAsset)
            {
                PanelViewToggle();

                if (_showXmlView)
                    PanelXmlEditor();
                else
                    PanelObjectEditor();

                PanelButtonXmlEditor();
            }
            else
            {
                EditorGUILayout.HelpBox("Select an XML file to edit", MessageType.Info);
            }
        }

        private void PanelViewToggle()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                _showXmlView = GUILayout.Toggle(_showXmlView, "XML View", "Button", GUILayout.Width(100));
                _showXmlView = !GUILayout.Toggle(!_showXmlView, "Object View", "Button", GUILayout.Width(100));
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void PanelObjectEditor()
        {
            if (_infoUtility.DeserializedEntity == null)
            {
                ShowErrorMessage();
                return;
            }

            var textStyleComponent = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
            };

            var textStyleHeader = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
            };

            EditorGUILayout.LabelField("Entity Properties", textStyleHeader);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                DrawFieldsUtility.DrawFieldsForObject(_infoUtility.DeserializedEntity);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Components", textStyleHeader);

            foreach (var component in _infoUtility.DeserializedEntity.GetSerializeComponents())
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(component.GetType().Name, textStyleComponent);
                    DrawFieldsUtility.DrawFieldsForObject(component);
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void PanelXmlEditor()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("XML Content (Read Only)", EditorStyles.boldLabel);

            var styleWindow = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true,
                normal = new GUIStyleState() { textColor = Color.gray }
            };

            EditorGUILayout.SelectableLabel(_infoUtility.XMLText, styleWindow, GUILayout.ExpandHeight(true));
        }

        private void ShowErrorMessage()
        {
            if (_infoUtility.DeserializedEntity == null)
            {
                EditorGUILayout.HelpBox("Failed to deserialize the object. Please ensure:", MessageType.Error);
                EditorGUILayout.HelpBox(
                    "1. This is a concrete type inheriting from CMSEntity\n" +
                    "2. The class has a default constructor\n" +
                    "3. All used components are serializable\n" +
                    "4. The XML file contains valid data",
                    MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("The object contains no serializable properties", MessageType.Warning);
            }
        }

        private void PanelFileXmlEditor()
        {
            EditorGUILayout.BeginHorizontal();
            {
                var fileInField = EditorGUILayout.ObjectField("XML File", _infoUtility.SelectedXmlAsset, typeof(TextAsset), false) as TextAsset;

                if (fileInField && fileInField != _infoUtility.SelectedXmlAsset && UnityXmlConverter.IsXmlFile(fileInField))
                {
                    _infoUtility.SelectedXmlAsset = fileInField;
                    _infoUtility.XMLText = _infoUtility.SelectedXmlAsset.text;
                    _parentWindow.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void PanelButtonXmlEditor()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save"))
                {
                    if (_infoUtility.DeserializedEntity != null)
                    {
                        _infoUtility.XMLText = SerializerUtility.TrySerialize(
                            _infoUtility.DeserializedEntity,
                            AssetDatabase.GetAssetPath(_infoUtility.SelectedXmlAsset));
                        AssetDatabase.Refresh();
                        UpdateInfo();
                        _parentWindow.Repaint();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
