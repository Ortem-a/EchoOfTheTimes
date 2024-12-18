using Systems.Movement;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GraphVisibility))]
public class GraphVisibilityEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        GraphVisibility graph = (GraphVisibility)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Reset And Load"))
        {
            graph.ResetAndLoad();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Reset Vertices"))
        {
            graph.ResetVertices();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Load"))
        {
            graph.Load();
        }
    }
}