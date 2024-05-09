#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Editor
{
    [CustomEditor(typeof(StairsCreator))]
    public class StairsCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            StairsCreator creator = (StairsCreator)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Create Stairs"))
            {
                creator.Create();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add States To Stairs"))
            {
                creator.AddStatesToStairs();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Delete Stairs"))
            {
                creator.Despawn();
            }
        }
    }
}
#endif