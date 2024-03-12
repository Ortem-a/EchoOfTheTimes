using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.Editor
{
    [CustomEditor(typeof(StairsCreator))]
    public class StairsCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            StairsCreator creator = (StairsCreator)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Create Or Update"))
            {
                creator.CreateOrUpdate();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Default States"))
            {
                creator.AddDefaultStates();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Default Vertices"))
            {
                creator.AddDefaultVertices();
            }
        }
    }
}