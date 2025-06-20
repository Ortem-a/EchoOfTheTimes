using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Leveling;
using Systems.Movement;
using Systems.Tools;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomEditor(typeof(StairsCreator))]
    public class StairsCreatorEditor : Editor
    {
        private List<Stair> _stairs;
        private Vector3 _realScale;
        private List<Vector3> _flatBottomPositions;

        private int _stairsNumber = 6;
        private float _stairHeight = 2;
        private float _vertexElevationUponStair = 0.5f;
        private Stair _stairPrefab;
        private Vertex _vertexPrefab;

        private bool _prefabsAvailable;

        private int _debugStateId;

        public override void OnInspectorGUI()
        {
            StairsCreator stairsCreator = (StairsCreator)target;

            if (stairsCreator.Stairs != null)
            {
                _stairs = stairsCreator.Stairs.ToList();
            }

            DrawDefaultInspector();

            EditorGUILayout.Space();

            _prefabsAvailable = true;

            var headerStyle = new GUIStyle();
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            GUILayout.Label("Stair Parameters", headerStyle);

            _stairsNumber = EditorGUILayout.IntField("Stairs Number", _stairsNumber);
            _stairHeight = EditorGUILayout.FloatField("Stair Height", _stairHeight);
            _vertexElevationUponStair = EditorGUILayout.FloatField("Vertex Elevation Upon Stair", _vertexElevationUponStair);

            _stairPrefab = AssetDatabase.LoadAssetAtPath<Stair>(@"Assets/_MEGAGIGAREFACTOR/Stairs/Step 1 by 6.prefab");
            if (_stairPrefab == null)
            {
                EditorGUILayout.HelpBox("Stair prefab not found at 'Assets/_MEGAGIGAREFACTOR/Stairs/Step 1 by 6.prefab'!",
                    MessageType.Error);

                _prefabsAvailable = false;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Stair Prefab");
                _stairPrefab = (Stair)EditorGUILayout.ObjectField(_stairPrefab, typeof(Stair), false);
                EditorGUILayout.EndHorizontal();
            }

            _vertexPrefab = AssetDatabase.LoadAssetAtPath<Vertex>(@"Assets/_MEGAGIGAREFACTOR/Stairs/StairVertex.prefab");
            if (_vertexPrefab == null)
            {
                EditorGUILayout.HelpBox("Stair vertex prefab not found at 'Assets/_MEGAGIGAREFACTOR/Stairs/StairVertex.prefab'!",
                    MessageType.Error);

                _prefabsAvailable = false;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Stair Vertex Prefab");
                _vertexPrefab = (VertexVisibility)EditorGUILayout.ObjectField(_vertexPrefab, typeof(VertexVisibility), false);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (_prefabsAvailable)
            {
                if (GUILayout.Button("Create"))
                {
                    Create(stairsCreator);
                }
                EditorGUILayout.Space();

                if (GUILayout.Button("Despawn"))
                {
                    Despawn(stairsCreator);
                }
                EditorGUILayout.Space();

                GUILayout.Label("DEBUG", headerStyle);

                _debugStateId = EditorGUILayout.IntField("Debug State Id", _debugStateId);

                if (GUILayout.Button("Transform To State"))
                {
                    TransformToState(stairsCreator);
                }
                EditorGUILayout.Space();
            }
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(StairsCreator stairsCreator, GizmoType gizmoType)
        {

        }

        private void Create(StairsCreator stairsCreator)
        {
            Despawn(stairsCreator);

            SpawnStairs(stairsCreator);

            MoveStairsToPlaces();

            AddVerteticesToStairs();

            AddStatesToStairs(stairsCreator);

            stairsCreator.Stairs = _stairs.ToArray();
        }

        private void SpawnStairs(StairsCreator stairsCreator)
        {
            _stairs = new List<Stair>();

            for (int i = 0; i < _stairsNumber; i++)
            {
                var stair = Instantiate(_stairPrefab, stairsCreator.transform);

                _stairs.Add(stair);
            }

            SetRealSize();
        }

        private void MoveStairsToPlaces()
        {
            _flatBottomPositions = new List<Vector3>();

            Vector3 position = _stairs[0].transform.localPosition;
            position.z += _realScale.z / 2f;
            _stairs[0].transform.localPosition = position;

            _flatBottomPositions.Add(position);

            for (int i = 1; i < _stairs.Count; i++)
            {
                position += Vector3.forward * _realScale.z;

                _stairs[i].transform.localPosition = position;

                _flatBottomPositions.Add(position);
            }
        }

        private void AddStatesToStairs(StairsCreator stairsCreator)
        {
            AddStatesForFlatBottom(stairsCreator.StairsStates);
            AddStatesForFlatTop(stairsCreator.StairsStates);
            AddStatesForStartBottom(stairsCreator.StairsStates);
            AddStatesForStartTop(stairsCreator.StairsStates);
        }

        private void AddStatesForFlatBottom(List<StairsStates> stairsStates)
        {
            var flatBottomIds = stairsStates.Find(x => x.StairState == StairStateType.FlatBottom);

            if (flatBottomIds != null)
            {
                SetOrUpdateState(flatBottomIds.StateIds);
                ResetPositions();
            }
        }

        private void AddStatesForFlatTop(List<StairsStates> stairsStates)
        {
            var flatTopIds = stairsStates.Find(x => x.StairState == StairStateType.FlatTop);

            if (flatTopIds != null)
            {
                for (int i = 0; i < _stairs.Count; i++)
                {
                    _stairs[i].transform.localPosition += (_stairsNumber - 1) * _stairHeight * Vector3.up;
                }

                SetOrUpdateState(flatTopIds.StateIds);
                ResetPositions();
            }
        }

        private void AddStatesForStartBottom(List<StairsStates> stairsStates)
        {
            var startBottomIds = stairsStates.Find(x => x.StairState == StairStateType.StartBottom);

            if (startBottomIds != null)
            {
                for (int i = 0; i < _stairs.Count; i++)
                {
                    _stairs[i].transform.localPosition += (i + 1) * _stairHeight * Vector3.up;
                }

                SetOrUpdateState(startBottomIds.StateIds);
                ResetPositions();
            }
        }

        private void AddStatesForStartTop(List<StairsStates> stairsStates)
        {
            var startTopIds = stairsStates.Find(x => x.StairState == StairStateType.StartTop);

            if (startTopIds != null)
            {
                for (int i = 0; i < _stairs.Count; i++)
                {
                    _stairs[i].transform.localPosition += (_stairs.Count - i) * _stairHeight * Vector3.up;
                }

                SetOrUpdateState(startTopIds.StateIds);
                ResetPositions();
            }
        }

        private void SetOrUpdateState(int[] ids)
        {
            throw new NotImplementedException();

            //foreach (int id in ids)
            //{
            //    for (int i = 0; i < _stairs.Count; i++)
            //    {
            //        _stairs[i].SetOrUpdateState(id);
            //    }
            //}
        }

        private void ResetPositions()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.localPosition = _flatBottomPositions[i];
            }
        }

        private void AddVerteticesToStairs()
        {
            var vertex = Instantiate(_vertexPrefab, _stairs[0].transform);
            vertex.transform.localPosition = Vector3.up * _vertexElevationUponStair;

            vertex = Instantiate(_vertexPrefab, _stairs[^1].transform);
            vertex.transform.localPosition = Vector3.up * _vertexElevationUponStair;
        }

        private void Despawn(StairsCreator stairsCreator)
        {
            _stairs = GetOrFindStairs(stairsCreator);

            if (_stairs != null)
            {
                for (int i = 0; i < _stairs.Count; i++)
                {
                    DestroyImmediate(_stairs[i].gameObject);
                }

                _stairs.Clear();
                _stairs = null;

            }

            stairsCreator.Stairs = null;
        }

        private void TransformToState(StairsCreator stairsCreator)
        {
            _stairs = stairsCreator.Stairs.ToList();

            foreach (var stair in _stairs)
            {
                if (!stair.TryAcceptStateImmediate(_debugStateId))
                {
                    Debug.LogError($"There is no state with Id '{_debugStateId}'");
                }
            }
        }

        private void SetRealSize()
        {
            _realScale = _stairs[0].GetComponent<MeshFilter>().sharedMesh.bounds.size;

            _stairHeight = _realScale.y / 6f;
        }

        private List<Stair> GetOrFindStairs(StairsCreator stairsCreator)
        {
            _stairs ??= stairsCreator.GetComponentsInChildren<Stair>().ToList();

            return _stairs;
        }
    }
}