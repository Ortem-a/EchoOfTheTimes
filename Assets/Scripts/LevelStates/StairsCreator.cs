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
        private Stair _stairPrefab;

        [Header("Add States For")]
        [SerializeField]
        private bool _flatBottom;
        [SerializeField]
        private int _flatBottomId;
        [SerializeField]
        private bool _flatTop;
        [SerializeField]
        private int _flatTopId;
        [SerializeField]
        private bool _startBottom;
        [SerializeField]
        private int _startBottomId;
        [SerializeField]
        private bool _startTop;
        [SerializeField]
        private int _startTopId;

        private List<Stair> _stairs;

        private List<Vector3> _flatBottomPositions;

        private Vector3 _realScale;

        public void Create()
        {
            Despawn();

            SpawnStairs();

            MoveStairsToPlaces();
        }

        private void SpawnStairs()
        {
            _stairs = new List<Stair>();

            for (int i = 0; i < _stairsNumber; i++)
            {
                var stair = Instantiate(_stairPrefab, transform);

                _stairs.Add(stair);
            }

            SetRealSize();
        }

        private void SetRealSize()
        {
            Vector3[] vertices = _stairs[0].GetComponent<MeshFilter>().sharedMesh.vertices;
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

            Vector3 position = _stairs[0].transform.position;
            position.z += _realScale.z / 2f;
            _stairs[0].transform.position = position;

            _flatBottomPositions.Add(position);

            for (int i = 1; i < _stairs.Count; i++)
            {
                position += Vector3.forward * _realScale.z;

                _stairs[i].transform.position = position;

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
            SetOrUpdateState(_flatBottomId);
            ResetPositions();
        }

        private void AddStatesForFlatTop()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.position += Vector3.up * _stairHeight * (_stairsNumber - 1);
            }

            SetOrUpdateState(_flatTopId);
            ResetPositions();
        }

        private void AddStatesForStartBottom()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.position += Vector3.up * _stairHeight * i;
            }

            SetOrUpdateState(_startBottomId);
            ResetPositions();
        }

        private void AddStatesForStartTop()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.position += Vector3.up * _stairHeight * (_stairs.Count - i - 1);
            }

            SetOrUpdateState(_startTopId);
            ResetPositions();
        }

        private void SetOrUpdateState(int id)
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].SetOrUpdateState(id);
            }
        }

        private void ResetPositions()
        {
            for (int i = 0; i < _stairs.Count; i++)
            {
                _stairs[i].transform.position = _flatBottomPositions[i];
            }
        }

        private List<Stair> GetOrFindStairs()
        {
            _stairs ??= GetComponentsInChildren<Stair>().ToList();

            return _stairs;
        }

        public void Despawn()
        {
            _stairs = GetOrFindStairs();

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
    }
}