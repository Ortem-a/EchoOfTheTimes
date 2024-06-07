#if UNITY_EDITOR
using EchoOfTheTimes.LevelStates.Local;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(LocalStateable))]
    public class LocalStateableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LocalStateable stateable = (LocalStateable)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Set Or Update Params To State"))
            {
                stateable.SetOrUpdateParamsToState();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Transform Object By State"))
            {
                stateable.TransformObjectByState();
            }
        }
    }
}
#endif