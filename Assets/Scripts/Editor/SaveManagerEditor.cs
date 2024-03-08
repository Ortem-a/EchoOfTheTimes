using EchoOfTheTimes.Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.Editor
{
    [CustomEditor(typeof(SaveLoadSystem))]
    public class SaveManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaveLoadSystem saveLoadSystem = (SaveLoadSystem)target;
            string gameName = saveLoadSystem.GameData.Name;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save Game"))
            {
                saveLoadSystem.SaveGame();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Load Game"))
            {
                saveLoadSystem.LoadGame(gameName);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Reload Game"))
            {
                saveLoadSystem.ReloadGame();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("New Game"))
            {
                saveLoadSystem.NewGame();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Delete Game"))
            {
                saveLoadSystem.DeleteGame(gameName);
            }
        }
    }
}