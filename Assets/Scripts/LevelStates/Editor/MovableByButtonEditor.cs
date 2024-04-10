#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(MovableByButton))]
    public class MovableByButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            MovableByButton movable = (MovableByButton)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Set Or Update Params"))
            {
                movable.SetOrUpdateParams();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Transform Object By Params"))
            {
                movable.TransformObjectByParams();
            }
        }
    }
}
#endif