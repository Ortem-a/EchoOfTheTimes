#if UNITY_EDITOR
using EchoOfTheTimes.LevelStates.Local;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(LocalMovableByRule))]
    public class LocalMovableByRuleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LocalMovableByRule movable = (LocalMovableByRule)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Set Or Update Params To Rule"))
            {
                movable.SetOrUpdateParamsToRule();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Transform Object By Rule"))
            {
                movable.TransformObjectByRule();
            }
        }
    }
}
#endif