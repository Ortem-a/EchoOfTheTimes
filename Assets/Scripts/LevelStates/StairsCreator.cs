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
        [SerializeField]
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

        public List<Stair> Stairs { get; private set; }

        private List<Vector3> _flatBottomPositions;

        private Vector3 _realScale;

        private void Awake()
        {
            Stairs = GetOrFindStairs();
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
            Vector3[] vertices = Stairs[0].GetComponent<MeshFilter>().sharedMesh.vertices;
            Vector3 refVert = vertices[0];

            var v = vertices.Where((vert) => vert.y == refVert.y).Distinct().ToList();

            var xs = v.Where((vert) => vert.x == v[0].x).ToList();
            var zs = v.Where((vert) => vert.z == v[0].z).ToList();

            Vector3 xDir = xs[1] - xs[0];
            Vector3 zDir = zs[1] - zs[0];

            var ys = vertices.Where((vert) => vert.x == refVert.x && vert.z == refVert.z).Distinct().ToList();
            Vector3 yDir = ys[1] - ys[0];

            _realScale.x = Mathf.Abs(zDir.x);
            _realScale.y = Mathf.Abs(yDir.y);
            _realScale.z = Mathf.Abs(xDir.z);
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

        private void AddStatesForFlatBottom()
        {
            SetOrUpdateState(FlatBottomIds);
            ResetPositions();
        }

        private void AddStatesForFlatTop()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.position += Vector3.up * _stairHeight * (_stairsNumber - 1);
            }

            SetOrUpdateState(FlatTopIds);
            ResetPositions();
        }

        private void AddStatesForStartBottom()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.position += Vector3.up * _stairHeight * i;
            }

            SetOrUpdateState(StartBottomIds);
            ResetPositions();
        }

        private void AddStatesForStartTop()
        {
            for (int i = 0; i < Stairs.Count; i++)
            {
                Stairs[i].transform.position += Vector3.up * _stairHeight * (Stairs.Count - i - 1);
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
            vertex.transform.localPosition = Vector3.up * (_realScale.y / 2f + _vertexElevationUponStair);

            vertex = Instantiate(_vertexPrefab, Stairs[^1].transform);
            vertex.transform.localPosition = Vector3.up * (_realScale.y / 2f + _vertexElevationUponStair);
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
    }
}