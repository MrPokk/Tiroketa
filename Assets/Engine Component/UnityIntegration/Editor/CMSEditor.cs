#if UNITY_EDITOR
using Engine_Component.UnityIntegration.Editor;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Engine_Component.CMSSystem
{
    public class CMSEditor : EditorWindow
    {
        // The order of rendering depends on the index
        private readonly static Dictionary<(int index, string name), CMSWindowTab> Tabs = new Dictionary<(int, string), CMSWindowTab>()
        {
            { (0, "Database"), new CMSDatabaseWindow() },
            { (1, "Inspector"), new CMSInspectorWindow() },
            { (2, "Settings"), new CMSSettingWindow() }
        };
        private int _currentTabIndex = 0;
        private bool _forceUpdate = false;

        private Vector2 _scrollPosition;

        private static void UpdateDatabase(bool forceUpdate = false)
        {
            CMSViewDatabase.FindAll(forceUpdate);
            CMSEntityDatabase.FindAll(forceUpdate);
            AssetDatabase.Refresh();
        }

        [MenuItem("CMS/CMS CENTER")]
        public static void ShowWindow()
        {


            var window = GetWindow<CMSEditor>("CMS Center");
            window.minSize = new Vector2(400, 500);
            UpdateDatabase();
        }

        private void OnEnable()
        {
            UpdateDatabase();

            foreach (var tabs in Tabs.Values)
            {
                tabs.OnEnable(this);
            }
        }

        private void OnSelectionChange()
        {
            foreach (var tabs in Tabs.Values)
            {
                tabs.OnSelectionChange();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawHeader();
                DrawSlider();
                DrawTabButtons();
                DrawSlider();
                DrawCurrentTab();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawHeader()
        {
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Refresh Database", GUILayout.Width(150), GUILayout.Height(25)))
                    UpdateDatabase(_forceUpdate);

                GUILayout.FlexibleSpace();

            }
            EditorGUILayout.EndHorizontal();

            _forceUpdate = EditorGUILayout.ToggleLeft("Force Update", _forceUpdate, GUILayout.Width(100));
        }

        private void DrawTabButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                foreach (var tabKey in Tabs.Keys.OrderBy(x => x.index))
                {
                    if (GUILayout.Toggle(_currentTabIndex == tabKey.index, tabKey.name, "Button", GUILayout.MinWidth(80)))
                    {
                        _currentTabIndex = tabKey.index;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCurrentTab()
        {
            var currentTabKey = Tabs.Keys.FirstOrDefault(x => x.index == _currentTabIndex);

            if (currentTabKey != default && Tabs.TryGetValue(currentTabKey, out var currentTab))
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        currentTab?.Draw();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox($"Tab with index {_currentTabIndex} is not available", MessageType.Warning);
            }
        }

        private void DrawSlider(int padding = 1)
        {
            EditorGUILayout.Space(padding);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space(padding);
        }
    }
}
#endif
