using System.Collections.Generic;
using System.Linq;
using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomEditor(typeof(LevelButton))]
    public class LevelButtonEditor : Editor
    {
        private static int _stateableByButtonIndex;
        private static List<StateableByButton> _avaliableStateablesByButton;

        public override void OnInspectorGUI()
        {
            LevelButton levelButton = (LevelButton)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            var allStateablesByButton = FindObjectsOfType<StateableByButton>();

            _avaliableStateablesByButton = new List<StateableByButton>();

            for (int i = 0; i < allStateablesByButton.Length; i++)
            {
                _avaliableStateablesByButton.Add(allStateablesByButton[i]);
            }

            var contents = new GUIContent[_avaliableStateablesByButton.Count];

            for (int i = 0; i < contents.Length; i++)
            {
                contents[i] = new GUIContent($"{_avaliableStateablesByButton[i].name}");
            }

            GUILayout.Label("Connect with:");

            _stateableByButtonIndex = EditorGUILayout.Popup(_stateableByButtonIndex, contents);

            EditorGUILayout.Space();

            if (GUILayout.Button("Connect"))
            {
                var stateables = levelButton.Stateables.ToList();

                if (stateables.Contains(_avaliableStateablesByButton[_stateableByButtonIndex]))
                {
                    var index = stateables.FindIndex((s) => s == _avaliableStateablesByButton[_stateableByButtonIndex]);

                    stateables[index] = _avaliableStateablesByButton[_stateableByButtonIndex];

                    Debug.Log($"{_avaliableStateablesByButton[_stateableByButtonIndex].name} updated to {levelButton.name}!");
                }
                else
                {
                    stateables.Add(_avaliableStateablesByButton[_stateableByButtonIndex]);

                    Debug.Log($"{_avaliableStateablesByButton[_stateableByButtonIndex].name} added to {levelButton.name}!");
                }

                levelButton.Stateables = stateables.ToArray();
            }

            EditorGUILayout.Space();
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(LevelButton levelButton, GizmoType gizmoType)
        {
            if (_avaliableStateablesByButton != null &&
                _avaliableStateablesByButton.Count != 0 &&
                _avaliableStateablesByButton[_stateableByButtonIndex] != null)
            {
                Color color = Color.magenta;

                Vector3 point1 = levelButton.transform.position;
                Vector3 point2 = _avaliableStateablesByButton[_stateableByButtonIndex].transform.position;

                GizmosDrawerHelper.DrawBezierCurveBetween(point1, point2, Vector3.up * 4f, 20, color);
                Gizmos.DrawSphere(point2, 0.5f);
            }
        }
    }
}