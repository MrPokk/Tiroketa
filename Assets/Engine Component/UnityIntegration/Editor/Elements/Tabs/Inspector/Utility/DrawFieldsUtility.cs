using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Engine_Component.UnityIntegration.Editor
{
    static class DrawFieldsUtility
    {
        public static void DrawFieldsForObject(object target)
        {
            if (target == null) return;

            var fields = target.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy
            );

            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(XmlIgnoreAttribute)))
                    continue;

                EditorGUI.BeginChangeCheck();
                var currentValue = field.GetValue(target);
                var newValue = DrawField(field.Name, currentValue, field.FieldType);

                if (EditorGUI.EndChangeCheck())
                {
                    field.SetValue(target, newValue);
                }
            }
        }

        public static object DrawField(string label, object value, Type type)
        {
            if (type == null)
            {
                EditorGUILayout.LabelField(label, "Type is null");
                return value;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String:
                    return EditorGUILayout.TextField(label, (string)value);
                case TypeCode.Int32:
                    return EditorGUILayout.IntField(label, (int)value);
                case TypeCode.Single:
                    return EditorGUILayout.FloatField(label, (float)value);
                case TypeCode.Boolean:
                    return EditorGUILayout.Toggle(label, (bool)value);

                default:
                    if (type.IsEnum)
                        return EditorGUILayout.EnumPopup(label, (Enum)value);
                    else if (type.IsArray)
                        return DrawArrayField(label, value, type);
                    else if (IsGenericList(type))
                        return DrawListField(label, value, type);
                    else if (type.IsClass)
                    {
                        EditorGUILayout.LabelField($"{label} (Unsupported class type: {type.Name})");
                        return value;
                    }
                    else
                    {
                        EditorGUILayout.LabelField(label, $"Unsupported type: {type.Name}");
                        return value;
                    }
            }
        }

        private static bool IsGenericList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        private static object DrawArrayField(string label, object value, Type arrayType)
        {
            var elementType = arrayType.GetElementType();
            var array = (Array)value;

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                var currentSize = array?.Length ?? 0;
                var newSize = Mathf.Max(0, EditorGUILayout.IntField("Size", currentSize));

                if ((array == null || newSize != currentSize) && elementType != null)
                    array = Array.CreateInstance(elementType, newSize);

                for (int i = 0; i < newSize; i++)
                {
                    array?.SetValue(DrawField($"Element {i}", array.GetValue(i), elementType), i);
                }
            }

            return array;
        }

        private static object DrawListField(string label, object value, Type listType)
        {
            var elementType = listType.GetGenericArguments()[0];
            var list = (IList)(value ?? Activator.CreateInstance(listType));

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var newValue = DrawField($"Element {i}", list[i], elementType);
                    if (!Equals(newValue, list[i]))
                        list[i] = newValue;
                }
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("+"))
                        list.Add(CreateDefault(elementType));

                    GUI.enabled = list.Count > 0;
                    if (GUILayout.Button("-"))
                        list.RemoveAt(list.Count - 1);
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }

            return list;
        }

        private static object CreateDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
