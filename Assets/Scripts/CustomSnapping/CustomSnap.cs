using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnap : MonoBehaviour
    {
        public float RotationAngle = 90f;
        [Space]
        [InspectorButton(nameof(Rotate))]
        public bool IsRotate;
        [Space]
        [Space]

        public GameObject SnapPointPrefab;
        public GameObject EdgePrefab;

        [Space]
        [InspectorButton(nameof(Accept))]
        public bool IsAccept;

        [Space]
        [InspectorButton(nameof(ResetPoints))]
        public bool IsResetPoints;

        [Space]
        [InspectorButton(nameof(ResetEdges))]
        public bool IsEdgesCreate;

        private Mesh _mesh;
        private Vector3[] _snapPointsPositions;

        private GameObject[] _spawnedSnapPoints = null;
        private GameObject[] _spawnedEdges = null;

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

        public void ResetEdges()
        {
            if (_spawnedSnapPoints != null)
            {
                DespawnEdges();

                List<GameObject> edges = new List<GameObject>();
                Vector3 middle;
                List<Vector3> middles = new List<Vector3>();

                int count = 0;
                for (int i = 0; i < _spawnedSnapPoints.Length; i++)
                {
                    for (int j = 0; j < _spawnedSnapPoints.Length; j++)
                    {
                        if (i == j) continue;

                        middle = (_spawnedSnapPoints[i].transform.localPosition + _spawnedSnapPoints[j].transform.localPosition) / 2f;

                        if (!middles.Contains(middle))
                        {
                            middles.Add(middle);

                            count++;
                            var obj = Instantiate(EdgePrefab, transform);
                            var edge = obj.GetComponent<CustomSnapEdge>();
                            edge.Head = _spawnedSnapPoints[i].GetComponent<CustomSnapPoint>();
                            edge.Tail = _spawnedSnapPoints[j].GetComponent<CustomSnapPoint>();
                            edge.Head.Edge = edge;
                            edge.Tail.Edge = edge;
                            obj.name = $"Edge_{count}";
                            obj.transform.localPosition = middle;
                            obj.transform.LookAt(_spawnedSnapPoints[i].transform);
                            edges.Add(obj);
                        }
                    }
                }

                _spawnedEdges = edges.ToArray();
            }
            else
            {
                ResetPoints();
                ResetEdges();
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

                var edgesPoints = transform.GetComponentsInChildren<CustomSnapEdge>();
                List<GameObject> edges = new List<GameObject>();
                foreach (var snapEdge in edgesPoints)
                {
                    snapEdge.Head.Edge = snapEdge;
                    snapEdge.Tail.Edge = snapEdge;

                    edges.Add(snapEdge.gameObject);
                }
                _spawnedEdges = edges.ToArray();
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

        private GameObject[] RemoveDuplicatesEdges(GameObject[] edges)
        {
            List<GameObject> distinctEdges = new List<GameObject>();

            for (int i = 0; i < edges.Length; i++)
            {
                for (int j = 0; j < edges.Length; j++)
                {
                    if (i == j) continue;

                    if (edges[i].transform.position != edges[j].transform.position)
                    {
                        distinctEdges.Add(edges[j]);
                    }
                }
            }

            return distinctEdges.ToArray();
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

        private void DespawnEdges()
        {
            if (_spawnedEdges != null)
            {
                foreach (var edge in _spawnedEdges)
                {
                    DestroyImmediate(edge);
                }

                _spawnedEdges = null;
            }
            else
            {
                Accept();
                if (_spawnedEdges != null)
                    DespawnEdges();
            }
        }

        private void OnDestroy()
        {
            DespawnEdges();
            DespawnPoints();
        }
    }
}