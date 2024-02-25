using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnap : MonoBehaviour
    {
        public GameObject SnapPointPrefab;

        [Space]
        [InspectorButton(nameof(InitializePoints))]
        public bool IsInitPoints;

        private Mesh _mesh;
        private Vector3[] _snapPointsPositions;

        private GameObject[] _spawnedSnapPoints = null;

        public void InitializePoints()
        {
            Despawn();

            _mesh = GetComponent<MeshFilter>().sharedMesh;

            _snapPointsPositions = _mesh.vertices;
            _snapPointsPositions = RemoveDuplicates();

            _spawnedSnapPoints = new GameObject[_snapPointsPositions.Length];

            int count = 0;
            for (int i = 0; i < _snapPointsPositions.Length; i++)
            {
                _spawnedSnapPoints[i] = Instantiate(SnapPointPrefab, transform);
                _spawnedSnapPoints[i].transform.localPosition = _snapPointsPositions[i];
                count++;
                _spawnedSnapPoints[i].name = $"SnapPoint_{count}";
            }
        }

        private Vector3[] RemoveDuplicates()
        {
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < _snapPointsPositions.Length; i++)
            {
                if (!positions.Contains(_snapPointsPositions[i]))
                {
                    positions.Add(_snapPointsPositions[i]);
                }
            }

            return positions.ToArray();
        }

        private void Despawn()
        {
            if (_spawnedSnapPoints != null)
            {
                foreach (var point in _spawnedSnapPoints)
                {
                    DestroyImmediate(point);
                }

                _spawnedSnapPoints = null;
            }
        }

        private void OnDestroy()
        {
            Despawn();
        }
    }
}