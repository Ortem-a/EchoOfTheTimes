using EchoOfTheTimes.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnap : MonoBehaviour
    {
        public float RotationAngle = 90f;
#if UNITY_EDITOR
        [Space]
        [InspectorButton(nameof(Rotate))]
        public bool IsRotate;
        [Space]
        [Space]
#endif

        public GameObject SnapPointPrefab;

#if UNITY_EDITOR
        [Space]
        [InspectorButton(nameof(Accept))]
        public bool IsAccept;

        [Space]
        [InspectorButton(nameof(ResetPoints))]
        public bool IsResetPoints;

#endif

        private Mesh _mesh;
        private Vector3[] _snapPointsPositions;

        private GameObject[] _spawnedSnapPoints = null;

        public void ResetPoints()
        {
            DespawnPoints();

            _mesh = GetComponent<MeshFilter>().sharedMesh;

            _snapPointsPositions = RemoveDuplicates(_mesh.vertices);

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

        public void Accept()
        {
            if (transform.childCount != 0)
            {
                var snapPoints = transform.GetComponentsInChildren<CustomSnapPoint>();
                List<GameObject> points = new List<GameObject>();
                foreach (var snapPoint in snapPoints)
                {
                    points.Add(snapPoint.gameObject);
                }
                _spawnedSnapPoints = points.ToArray();
            }
        }

        public void Rotate()
        {
            transform.Rotate(Vector3.up, RotationAngle);
        }

        private Vector3[] RemoveDuplicates(Vector3[] array)
        {
            List<Vector3> newArray = new List<Vector3>();

            for (int i = 0; i < array.Length; i++)
            {
                if (!newArray.Contains(array[i]))
                {
                    newArray.Add(array[i]);
                }
            }

            return newArray.ToArray();
        }

        private void DespawnPoints()
        {
            if (_spawnedSnapPoints != null)
            {
                foreach (var point in _spawnedSnapPoints)
                {
                    DestroyImmediate(point);
                }

                _spawnedSnapPoints = null;
            }
            else
            {
                Accept();
                if (_spawnedSnapPoints != null)
                    DespawnPoints();
            }
        }

        private void OnDestroy()
        {
            DespawnPoints();
        }
    }
}