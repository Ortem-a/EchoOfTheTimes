#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(Stateable))]
    public class StateableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Stateable stateable = (Stateable)target;

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