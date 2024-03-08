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

            

            EditorGUILayout.Space();

            if (GUILayout.Button("Set Or Update Params To Special State"))
            {
                stateable.SetOrUpdateParamsToSpecialState();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Transform Object By Special State"))
            {
                stateable.TransformObjectBySpecialState();
            }
        }
    }
}