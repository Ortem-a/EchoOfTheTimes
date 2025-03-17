using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomPropertyDrawer(typeof(Systems.RequiredFieldAttribute))]
    public class RequiredFieldPropertyDrawer : PropertyDrawer
    {
        private Texture2D _requiredIcon = EditorGUIUtility.IconContent("console.erroricon").image as Texture2D;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            Rect fieldRect = new Rect(position.x, position.y, position.width - 20f, position.height);
            EditorGUI.PropertyField(fieldRect, property, label);

            // if the field is required, but unassigned, show the icon
            if (IsFieldUnassigned(property))
            {
                Rect iconRect = new Rect(position.xMax - 18f, fieldRect.y, 16f, 16f);
                GUI.Label(iconRect,
                    new GUIContent(_requiredIcon, "This field is required and is either missing or emtpy!"));
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);

                // force a repaint of the hierarchy
                EditorApplication.RepaintHierarchyWindow();
            }

            EditorGUI.EndProperty();
        }

        private bool IsFieldUnassigned(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference when property.objectReferenceValue:
                case SerializedPropertyType.ExposedReference when property.exposedReferenceValue:
                case SerializedPropertyType.AnimationCurve when property.animationCurveValue is { length: > 0 }:
                case SerializedPropertyType.String when !string.IsNullOrEmpty(property.stringValue):
                    return false;
                default:
                    return true;
            }
        }
    }
}