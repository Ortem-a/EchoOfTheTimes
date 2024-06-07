#if UNITY_EDITOR
using EchoOfTheTimes.LevelStates.Local;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(LocalMovableByButton))]
    public class LocalMovableByButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LocalMovableByButton movable = (LocalMovableByButton)target;

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