using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.ScriptableObjects;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class StateableGizmosDrawer : MonoBehaviour
    {
#if UNITY_EDITOR
        private ColorStateSettingsScriptableObject _colorStateSettings;

        private Stateable _stateable;
        private Mesh _mesh;
        private List<(Mesh m, Transform t)> _meshes;

        private void OnDrawGizmosSelected()
        {
            if (_stateable != null && _colorStateSettings != null)
            {
                DrawStates();
                DrawSpecialStates();
            }
            else
            {
                InitComponents();
            }
        }

        private void InitComponents()
        {
            _stateable = GetComponent<Stateable>();

            if (TryGetComponent(out MeshFilter mf))
            {
                _mesh = mf.sharedMesh;
            }
            else
            {
                _meshes = new List<(Mesh m, Transform t)>();
                var filters = GetComponentsInChildren<MeshFilter>();

                foreach (var filter in filters)
                {
                    _meshes.Add((filter.sharedMesh, filter.transform));
                }
            }

            _colorStateSettings = AssetDatabase.LoadAssetAtPath<ColorStateSettingsScriptableObject>(
                @"Assets/ScriptableObjects/ColorStateSettings.asset");
        }

        private void DrawStates()
        {
            if (_stateable.States != null)
            {
                foreach (var stateParameter in _stateable.States)
                {
                    Gizmos.color = _colorStateSettings.GetColor(stateParameter.StateId);

                    if (_mesh != null)
                    {
                        Gizmos.DrawWireMesh(_mesh, stateParameter.Position, Quaternion.Euler(stateParameter.Rotation), stateParameter.LocalScale);
                    }
                    else if (_meshes != null)
                    {
                        GizmosHelper.DrawWireMeshesByTRS(_meshes, stateParameter);
                    }
                    else
                    {
                        InitComponents();
                    }
                }
            }
        }

        private void DrawSpecialStates()
        {
            if (_stateable.SpecialTransitions != null)
            {
                foreach (var specTransition in _stateable.SpecialTransitions)
                {
                    foreach (var stateParameter in specTransition.Parameters)
                    {
                        Gizmos.color = _colorStateSettings.GetColor(stateParameter.StateId);

                        if (_mesh != null)
                        {
                            Gizmos.DrawWireMesh(_mesh, stateParameter.Position, Quaternion.Euler(stateParameter.Rotation), stateParameter.LocalScale);
                        }
                        else if (_meshes != null)
                        {
                            GizmosHelper.DrawWireMeshesByTRS(_meshes, stateParameter);
                        }
                        else
                        {
                            InitComponents();
                        }
                    }
                }
            }
        }
#endif
    }
}