using System.Collections.Generic;
using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomEditor(typeof(Teleportator))]
    public class TeleportatorEditor : Editor
    {
        private static int _teleportIndex;
        private static List<Teleportator> _avaliableTeleports;

        public override void OnInspectorGUI()
        {
            Teleportator teleportator = (Teleportator)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            var allTeleports = FindObjectsOfType<Teleportator>();

            _avaliableTeleports = new List<Teleportator>();

            for (int i = 0; i < allTeleports.Length; i++)
            {
                if (allTeleports[i] != teleportator)
                {
                    _avaliableTeleports.Add(allTeleports[i]);
                }
            }

            var contents = new GUIContent[_avaliableTeleports.Count];

            for (int i = 0; i < contents.Length; i++)
            {
                contents[i] = new GUIContent($"{_avaliableTeleports[i].name}_{_avaliableTeleports[i].Vertex.Id}");
            }

            GUILayout.Label("Connect with:");

            _teleportIndex = EditorGUILayout.Popup(_teleportIndex, contents);

            EditorGUILayout.Space();

            if (GUILayout.Button("Connect"))
            {
                teleportator.Destination = _avaliableTeleports[_teleportIndex];
                _avaliableTeleports[_teleportIndex].Destination = teleportator;

                Debug.Log($"{teleportator.Destination.Vertex} and {_avaliableTeleports[_teleportIndex].Destination.Vertex} are connected!");
            }

            EditorGUILayout.Space();
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(Teleportator teleportator, GizmoType gizmoType)
        {
            if (_avaliableTeleports != null && _avaliableTeleports[_teleportIndex] != null)
            {
                Color color = Color.magenta;

                Vector3 point1 = teleportator.Vertex.transform.position;
                Vector3 point2 = _avaliableTeleports[_teleportIndex].Vertex.transform.position;

                GizmosDrawerHelper.DrawBezierCurveBetween(point1, point2, Vector3.up * 2f, 20, color);
            }
        }
    }
}