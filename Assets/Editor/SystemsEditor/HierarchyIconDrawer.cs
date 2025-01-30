using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [InitializeOnLoad]
    public static class HierarchyIconDrawer
    {
        private static readonly Texture2D requiredIcon
            = EditorGUIUtility.IconContent("console.erroricon").image as Texture2D;

        private static readonly Dictionary<Type, FieldInfo[]> cachedFieldInfo = new Dictionary<Type, FieldInfo[]>();

        static HierarchyIconDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is not GameObject gameObject) return;

            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component == null) continue;

                var fields = GetCachedFieldsWithRequiredAttribute(component.GetType());
                if (fields == null) continue;

                if (fields.Any(field => IsFieldUnassigned(field.GetValue(component))))
                {
                    var inconRect = new Rect(selectionRect.xMax - 20f, selectionRect.y, 16f, 16f);
                    GUI.Label(inconRect,
                        new GUIContent(requiredIcon, "One or more required fields are missing or empty!"));
                    break;
                }
            }
        }

        private static FieldInfo[] GetCachedFieldsWithRequiredAttribute(Type componentType)
        {
            if (!cachedFieldInfo.TryGetValue(componentType, out FieldInfo[] fields))
            {
                fields = componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                List<FieldInfo> requiredFields = new List<FieldInfo>();

                foreach (FieldInfo field in fields)
                {
                    bool isSerialized = field.IsPublic || field.IsDefined(typeof(SerializeField), false);
                    bool isRequired = field.IsDefined(typeof(Systems.RequiredFieldAttribute), false);

                    if (isSerialized && isRequired)
                    {
                        requiredFields.Add(field);
                    }
                }

                fields = requiredFields.ToArray();
                cachedFieldInfo[componentType] = fields;
            }

            return fields;
        }

        private static bool IsFieldUnassigned(object fieldValue)
        {
            if (fieldValue == null) return true;

            if (fieldValue is string stringValue && string.IsNullOrEmpty(stringValue)) return true;

            if (fieldValue is System.Collections.IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item == null) return true;
                }
            }

            return false;
        }
    }
}