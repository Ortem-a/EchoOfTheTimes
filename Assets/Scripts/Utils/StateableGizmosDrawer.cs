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
                        Gizmos.DrawWireMesh(_mesh, stateParameter.Position, Quaternion.Euler(stateParameter.Rotation), stateParameter.LocalScale);
                    }
                }
            }
            else
            {
                _stateable = GetComponent<Stateable>();
                _mesh = GetComponent<MeshFilter>().sharedMesh;
                _colorStateSettings = 
                    AssetDatabase.LoadAssetAtPath<ColorStateSettingsScriptableObject>(@"Assets/ScriptableObjects/ColorStateSettings.asset");
            }
        }
    }
}