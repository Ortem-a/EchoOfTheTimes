using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class StateableGizmosDrawer : MonoBehaviour
    {
        private ColorStateSettingsScriptableObject _colorStateSettings;

        private Stateable _stateable;
        private Mesh _mesh;

        [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
        private void OnDrawGizmosSelected()
        {
            if (_stateable != null && _colorStateSettings != null && _mesh != null)
            {
                if (_stateable.States != null)
                {
                    foreach (var stateParameter in _stateable.States)
                    {
                        Gizmos.color = _colorStateSettings.GetColor(stateParameter.StateId);
                        //Gizmos.DrawWireCube(stateParameter.Position, stateParameter.LocalScale);
                        Gizmos.DrawWireMesh(_mesh, stateParameter.Position, Quaternion.Euler(stateParameter.Rotation), stateParameter.LocalScale);
                    }
                }
            }
            else
            {
                _stateable = GetComponent<Stateable>();
                _mesh = ConvertToLineMesh(GetComponent<MeshFilter>().sharedMesh);
                _colorStateSettings = AssetDatabase.LoadAssetAtPath<ColorStateSettingsScriptableObject>(@"Assets/ScriptableObjects/ColorStateSettings.asset");
            }
        }

        public static Mesh ConvertToLineMesh(Mesh mesh)
        {
            var lineMesh = new Mesh();
            var tris = mesh.triangles;
            List<int> lineIndices = new List<int>(tris.Length * 2);
            for (int i = 0; i < tris.Length; i += 3)
            {
                lineIndices.Add(tris[i]);
                lineIndices.Add(tris[i + 1]);

                lineIndices.Add(tris[i + 1]);
                lineIndices.Add(tris[i + 2]);

                lineIndices.Add(tris[i + 2]);
                lineIndices.Add(tris[i]);
            }
            lineMesh.vertices = mesh.vertices;
            lineMesh.uv = mesh.uv;
            lineMesh.normals = mesh.normals;
            lineMesh.tangents = mesh.tangents;
            lineMesh.SetIndices(lineIndices, MeshTopology.Lines, 0, true);
            return lineMesh;
        }
    }
}