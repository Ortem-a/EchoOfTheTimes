using EchoOfTheTimes.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class StairsCreator : MonoBehaviour
    {
        [Header("Stair Parameters")]
        [SerializeField]
        private int _stairsNumber;
        private float _stairHeight;
        [SerializeField]
        private float _vertexElevationUponStair;
        [SerializeField]
        private Stair _stairPrefab;
        [SerializeField]
        private Vertex _vertexPrefab;

        [Header("Add States For")]
        [SerializeField]
        private bool _flatBottom;
        [field: SerializeField]
        public List<int> FlatBottomIds { get; private set; }
        [SerializeField]
        private bool _flatTop;
        [field: SerializeField]
        public List<int> FlatTopIds { get; private set; }
        [SerializeField]
        private bool _startBottom;
        [field: SerializeField]
        public List<int> StartBottomIds { get; private set; }
        [SerializeField]
        private bool _startTop;
        [field: SerializeField]
        public List<int> StartTopIds { get; private set; }

        [Header("DEBUG")]
        [SerializeField]
        private int _debugStateId;

        public List<Stair> Stairs { get; private set; }

        private List<Vector3> _flatBottomPositions;

        private Vector3 _realScale;

        private void Awake()
        {
            Stairs = GetOrFindStairs();

            SetRealSize();

            _flatBottomPositions = new List<Vector3>();

            Vector3 position = Stairs[0].transform.localPosition;

            _flatBottomPositions.Add(position);

            for (int i = 1; i < Stairs.Count; i++)
            {
                position += Vector3.forward * _realScale.z;
                _flatBottomPositions.Add(position);
            }
        }

        public void Create()
        {
            Despawn();

            SpawnStairs();

            MoveStairsToPlaces();

            AddVerteticesToStairs();
        }

        private void SpawnStairs()
        {
            Stairs = new List<Stair>();

            for (int i = 0; i < _stairsNumber; i++)
            {
                var stair = Instantiate(_stairPrefab, transform);

                Stairs.Add(stair);
            }

            SetRealSize();
        }

        private void SetRealSize()
        {
            _realScale = Stairs[0].GetComponent<MeshFilter>().sharedMesh.bounds.size;

            _stairHeight = _realScale.y / 6f;
        }

        private void MoveStairsToPlaces()
        {
            _flatBottomPositions = new List<Vector3>();

            Vector3 position = Stairs[0].transform.localPosition;
            position.z += _realScale.z / 2f;
            Stairs[0].transform.localPosition = position;

            _flatBottomPositions.Add(position);

            for (int i = 1; i < Stairs.Count; i++)
            {
                position += Vector3.forward * _realScale.z;

                Stairs[i].transform.localPosition = position;

                _flatBottomPositions.Add(position);
            }
        }

        public void AddStatesToStairs()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].Initialize();
            }

            if (_flatBottom) AddStatesForFlatBottom();
            if (_flatTop) AddStatesForFlatTop();
            if (_startBottom) AddStatesForStartBottom();
            if (_startTop) AddStatesForStartTop();
        }

        public List<StateParameter> GetFlatBottomPositions()
        {
            return PositionsToStateParameters(_flatBottomPositions);
        }

        public List<StateParameter> GetFlatTopPositions()
        {
            List<Vector3> positions = _flatBottomPositions;
            Vector3 startPosition = _flatBottomPositions.Count * _stairHeight * Vector3.up;

            for (int i = 0; i < _flatBottomPositions.Count; i++)
            {
                positions[i] += startPosition;
            }

            return PositionsToStateParameters(positions);
        }

        public List<StateParameter> GetStartBottomPositions()
        {
            List<Vector3> positions = _flatBottomPositions;
            Vector3 startPosition;

            for (int i = 0; i < _flatBottomPositions.Count; i++)
            {
                startPosition = (i + 1) * _stairHeight * Vector3.up;

                positions[i] += startPosition;
            }

            return PositionsToStateParameters(positions);
        }

        public List<StateParameter> GetStartTopPositions()
        {
            List<Vector3> positions = _flatBottomPositions;
            Vector3 startPosition;

            for (int i = 0; i < _flatBottomPositions.Count; i++)
            {
                startPosition = (_flatBottomPositions.Count - i) * _stairHeight * Vector3.up;

                positions[i] += startPosition;
            }

            return PositionsToStateParameters(positions);
        }

        private List<StateParameter> PositionsToStateParameters(List<Vector3> positions)
        {
            List<StateParameter> parameters = new List<StateParameter>();
            StateParameter p;

            for (int i = 0; i < Stairs.Count; i++)
            {
                p = new StateParameter()
                {
                    Target = Stairs[i].transform,
                    Position = positions[i],
                    Rotation = Stairs[i].transform.localRotation.eulerAngles,
                    //Position = transform.TransformPoint(positions[i]),
                    //Rotation = Stairs[i].transform.rotation.eulerAngles,
                    LocalScale = Stairs[i].transform.localScale
                };

                parameters.Add(p);
            }

            return parameters;
        }

        private void AddStatesForFlatBottom()
        {
            SetOrUpdateState(FlatBottomIds);
            ResetPositions();
        }

        private void AddStatesForFlatTop()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.localPosition += (_stairsNumber - 1) * _stairHeight * Vector3.up;
            }

            SetOrUpdateState(FlatTopIds);
            ResetPositions();
        }

        private void AddStatesForStartBottom()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.localPosition += (i + 1) * _stairHeight * Vector3.up;
            }

            SetOrUpdateState(StartBottomIds);
            ResetPositions();
        }

        private void AddStatesForStartTop()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.localPosition += (Stairs.Count - i) * _stairHeight * Vector3.up;
            }

            SetOrUpdateState(StartTopIds);
            ResetPositions();
        }

        private void SetOrUpdateState(List<int> ids)
        {
            foreach (int id in ids)
            {
                for (int i = 0; i < Stairs.Count; i++)
                {
                    Stairs[i].SetOrUpdateState(id);
                }
            }
        }

        private void ResetPositions()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.localPosition = _flatBottomPositions[i];
            }
        }

        private void AddVerteticesToStairs()
        {
            var vertex = Instantiate(_vertexPrefab, Stairs[0].transform);
            vertex.transform.localPosition = Vector3.up * _vertexElevationUponStair;

            vertex = Instantiate(_vertexPrefab, Stairs[^1].transform);
            vertex.transform.localPosition = Vector3.up * _vertexElevationUponStair;
        }

        private List<Stair> GetOrFindStairs()
        {
            Stairs ??= GetComponentsInChildren<Stair>().ToList();

            return Stairs;
        }

        public void Despawn()
        {
            Stairs = GetOrFindStairs();

            if (Stairs != null)
            {
                for (int i = 0; i < Stairs.Count; i++)
                {
                    DestroyImmediate(Stairs[i].gameObject);
                }

                Stairs.Clear();
                Stairs = null;
            }
        }

        public void TransformToState()
        {
            foreach (var stair in Stairs)
            {
                stair.TransformStairsToState(_debugStateId);
            }
        }
    }
}