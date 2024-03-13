using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EchoOfTheTimes.Core;

namespace EchoOfTheTimes.Editor
{
    [CustomEditor(typeof(CheckpointManager))]
    public class CheckpointManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CheckpointManager checkpointManager = (CheckpointManager)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset checkpoints"))
            {
                checkpointManager.ResetCheckpoints();
            }
        }
    }
}