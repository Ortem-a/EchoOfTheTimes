using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.Collectables.Editor
{
    [CustomEditor(typeof(CollectableSpawner))]
    public class ItemSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var itemSpawner = (CollectableSpawner)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Find All Placeholders"))
            {
                var placeholders = FindObjectsOfType<CollectablePlaceholder>();

                if (placeholders.Length != 0)
                {
                    itemSpawner.SetPlaceholdersFromEditor(placeholders.ToList());
                }
                else
                {
                    Debug.Log($"There is no objects of type '{nameof(CollectablePlaceholder)}'");
                }
            }
        }
    }
}