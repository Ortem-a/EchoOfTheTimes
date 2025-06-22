using System.Collections;
using System.Collections.Generic;
using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomEditor(typeof(StateableByButton))]
    public class StateableByButtonEditor : Editor
    {
        private int _stateIdToSet;
        private int _stateIdToTransform;

        public override void OnInspectorGUI()
        {
            IStateable stateable = (IStateable)target;
            StateableByButton stateableObject = (StateableByButton)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            _stateIdToSet = EditorGUILayout.IntField("State Id", _stateIdToSet);

            if (GUILayout.Button("Set Or Update Params To State"))
            {
                stateable.SetOptionsForState(_stateIdToSet, Selection.activeTransform);

                EditorUtility.SetDirty(stateableObject);
            }

            EditorGUILayout.Space();

            _stateIdToTransform = EditorGUILayout.IntField("State Id", _stateIdToTransform);

            if (GUILayout.Button("Transform Object By State"))
            {
                if (stateable.TryGetOption(_stateIdToTransform, out var option))
                {
                    var selectedObject = Selection.activeTransform;

                    selectedObject.SetLocalPositionAndRotation(
                        option.LocalPosition, option.LocalRotation);
                    selectedObject.localScale = option.LocalScale;
                }
            }

            EditorGUILayout.Space();
        }
    }
}