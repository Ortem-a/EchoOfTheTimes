using EchoOfTheTimes.Core;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.Editor
{
    [CustomEditor(typeof(CheckPoint))]
    public class CheckPointEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CheckPoint checkPoint = (CheckPoint)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Set Point"))
            {
                checkPoint.SetCheckPoint();
            }
        }
    }
}