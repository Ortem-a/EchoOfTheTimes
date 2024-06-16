#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(MovableByRule))]
    public class MovableByRuleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            MovableByRule movable = (MovableByRule)target;

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