using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Leveling;
using Systems.Movement;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomEditor(typeof(BridgeService))]
    public class BridgeServiceEditor : UnityEditor.Editor
    {
        private static int _innerBridgeIndex;
        private static List<Vertex> _avaliableInnerBridges;

        private static int _outerBridgeIndex;
        private static List<Vertex> _avaliableOuterBridges;

        public override void OnInspectorGUI()
        {
            var bridgeService = (BridgeService)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (!TryCollectInners(bridgeService))
            {
                return;
            }

            CollectOuters();

            DrawInnerContents();

            DrawOuterContents();

            EditorGUILayout.Space();

            if (GUILayout.Button("Connect"))
            {
                var newBridge = new Bridge()
                {
                    Inner = _avaliableInnerBridges[_innerBridgeIndex],
                    Outer = _avaliableOuterBridges[_outerBridgeIndex]
                };

                var bridges = bridgeService.Bridges.ToList();

                if (bridges.Contains(newBridge))
                {
                    var index = bridges.FindIndex((b) => b.Equals(newBridge));
                    bridges[index] = newBridge;

                    Debug.Log($"Bridge between {newBridge.Inner} and {newBridge.Outer} updated!");
                }
                else
                {
                    bridges.Add(newBridge);

                    Debug.Log($"New bridge between {newBridge.Inner} and {newBridge.Outer} added!");
                }

                bridgeService.Bridges = bridges.ToArray();
            }

            EditorGUILayout.Space();
        }

        private void CollectOuters()
        {
            var allVertices = FindObjectsOfType<Vertex>();

            _avaliableOuterBridges = new List<Vertex>();

            for (int i = 0; i < allVertices.Length; i++)
            {
                if (allVertices[i].IsBridge && !_avaliableInnerBridges.Contains(allVertices[i]))
                {
                    _avaliableOuterBridges.Add(allVertices[i]);
                }
            }
        }

        private bool TryCollectInners(BridgeService bridgeService)
        {
            var allInnerVertices = bridgeService.GetComponentsInChildren<Vertex>();
            _avaliableInnerBridges = new List<Vertex>();

            for (int i = 0; i < allInnerVertices.Length; i++)
            {
                if (allInnerVertices[i].IsBridge)
                {
                    _avaliableInnerBridges.Add(allInnerVertices[i]);
                }
            }

            if (_avaliableInnerBridges.Count == 0)
            {
                EditorGUILayout.HelpBox("There is no bridges!", MessageType.Error);
                return false;
            }

            return true;
        }

        private void DrawInnerContents()
        {
            var innerContents = new GUIContent[_avaliableInnerBridges.Count];

            for (int i = 0; i < innerContents.Length; i++)
            {
                innerContents[i] = new GUIContent($"{_avaliableInnerBridges[i].name}_{_avaliableInnerBridges[i].Id}");
            }

            GUILayout.Label("Select Inner Bridge:");

            _innerBridgeIndex = EditorGUILayout.Popup(_innerBridgeIndex, innerContents);
        }

        private void DrawOuterContents()
        {
            var outerContents = new GUIContent[_avaliableOuterBridges.Count];

            for (int i = 0; i < outerContents.Length; i++)
            {
                outerContents[i] = new GUIContent($"{_avaliableOuterBridges[i].name}_{_avaliableOuterBridges[i].Id}");
            }

            GUILayout.Label("Select Outer Bridge:");

            _outerBridgeIndex = EditorGUILayout.Popup(_outerBridgeIndex, outerContents);
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(BridgeService bridgeService, GizmoType gizmoType)
        {
            Gizmos.color = Color.magenta;

            if (_avaliableInnerBridges != null &&
                _innerBridgeIndex <= _avaliableInnerBridges.Count && 
                _avaliableInnerBridges[_innerBridgeIndex] != null)
            {
                Gizmos.DrawCube(_avaliableInnerBridges[_innerBridgeIndex].transform.position, Vector3.one * 0.5f);
            }
            else
            {
                return;
            }

            if (_avaliableOuterBridges != null &&
                _outerBridgeIndex <= _avaliableOuterBridges.Count &&
                _avaliableOuterBridges[_outerBridgeIndex] != null)
            {
                Vector3 point1 = _avaliableInnerBridges[_innerBridgeIndex].transform.position;
                Vector3 point2 = _avaliableOuterBridges[_outerBridgeIndex].transform.position;

                GizmosDrawerHelper.DrawBezierCurveBetween(point1, point2, Vector3.up * 3f, 20, Color.magenta);
            }

            Gizmos.color = Color.white;
        }
    }
}
