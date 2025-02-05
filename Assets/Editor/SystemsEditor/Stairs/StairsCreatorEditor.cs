using System.Collections.Generic;
using System.Linq;
using Systems.Leveling;
using Systems.Movement;
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

        private int _stairsNumber;
        private float _stairHeight;
        private float _vertexElevationUponStair;
        private Stair _stairPrefab;
        private Vertex _vertexPrefab;

        private bool _flatBottom;
        private List<int> _flatBottomIds;
        private bool _flatTop;
        private List<int> _flatTopIds;
        private bool _startBottom;
        private List<int> _startBottomIds;
        private bool _startTop;
        private List<int> _startTopIds;

        private int _debugStateId;

        public override void OnInspectorGUI()
        {
            StairsCreator stairsCreator = (StairsCreator)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            var headerStyle = new GUIStyle();
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;

            GUILayout.Label("Stair Parameters", headerStyle);

            _stairsNumber = EditorGUILayout.IntField("Stairs Number", _stairsNumber);
            _stairHeight = EditorGUILayout.FloatField("Stair Height", _stairHeight);
            _vertexElevationUponStair = EditorGUILayout.FloatField("Vertex Elevation Upon Stair", _vertexElevationUponStair);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Stair Prefab");
            _stairPrefab = (Stair)EditorGUILayout.ObjectField(_stairPrefab, typeof(Stair), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Stair Vertex Prefab");
            _vertexPrefab = (VertexVisibility)EditorGUILayout.ObjectField(_vertexPrefab, typeof(VertexVisibility), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.Label("Add States For", headerStyle);

            _flatBottom = EditorGUILayout.Toggle("Flat Bottom", _flatBottom);
            _flatTop = EditorGUILayout.Toggle("Flat Top", _flatTop);
            _startBottom = EditorGUILayout.Toggle("Start Bottom", _startBottom);
            _startTop = EditorGUILayout.Toggle("Start Top", _startTop);

            EditorGUILayout.Space();

            GUILayout.Label("DEBUG", headerStyle);

            _debugStateId = EditorGUILayout.IntField("Debug State Id", _debugStateId);

            // TODO: need to add IDs

            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
            {
                Create(stairsCreator);
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Add States To Stairs"))
            {
                AddStatesToStairs();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Despawn"))
            {
                Despawn(stairsCreator);
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Transform To State"))
            {
                TransformToState();
            }
            EditorGUILayout.Space();
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(StairsCreator stairsCreator, GizmoType gizmoType)
        {

        }

        public void Create(StairsCreator stairsCreator)
        {
            Despawn(stairsCreator);

            SpawnStairs(stairsCreator);

            MoveStairsToPlaces();

            AddVerteticesToStairs();
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

        public void AddStatesToStairs()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].Initialize();
            }

            if (_flatBottom) AddStatesForFlatBottom();
            if (_flatTop) AddStatesForFlatTop();
            if (_startBottom) AddStatesForStartBottom();
            if (_startTop) AddStatesForStartTop();
        }

        private void AddStatesForFlatBottom()
        {
            SetOrUpdateState(_flatBottomIds);
            ResetPositions();
        }

        private void AddStatesForFlatTop()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.localPosition += (_stairsNumber - 1) * _stairHeight * Vector3.up;
            }

            SetOrUpdateState(_flatTopIds);
            ResetPositions();
        }

        private void AddStatesForStartBottom()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.localPosition += (i + 1) * _stairHeight * Vector3.up;
            }

            SetOrUpdateState(_startBottomIds);
            ResetPositions();
        }

        private void AddStatesForStartTop()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.localPosition += (_stairs.Count - i) * _stairHeight * Vector3.up;
            }

            SetOrUpdateState(_startTopIds);
            ResetPositions();
        }

        private void SetOrUpdateState(List<int> ids)
        {
            foreach (int id in ids)
            {
                for (int i = 0; i < _stairs.Count; i++)
                {
                    _stairs[i].SetOrUpdateState(id);
                }
            }
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

        public void Despawn(StairsCreator stairsCreator)
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
        }

        public void TransformToState()
        {
            foreach (var stair in _stairs)
            {
                stair.TransformStairsToState(_debugStateId);
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