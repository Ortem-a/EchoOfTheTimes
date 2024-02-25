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
        public GameObject SnapPointPrefab;
        public GameObject EdgePrefab;

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

                for (int i = 0; i < _spawnedSnapPoints.Length; i++)
                {
                    for (int j = 0; j < _spawnedSnapPoints.Length; j++)
                    {
                        if (i == j) continue;

                        middle = (_spawnedSnapPoints[i].transform.localPosition + _spawnedSnapPoints[j].transform.localPosition) / 2f;

                        if (!middles.Contains(middle))
                        {
                            middles.Add(middle);

                            var obj = Instantiate(EdgePrefab, transform);
                            var edge = obj.GetComponent<CustomSnapEdge>();
                            edge.Head = _spawnedSnapPoints[i].transform.localPosition;
                            edge.Tail = _spawnedSnapPoints[j].transform.localPosition;
                            obj.name = $"Edge_{i}_{j}";
                            obj.transform.localPosition = middle;
                            obj.transform.LookAt(_snapPointsPositions[i]);
                            edges.Add(obj);
                        }
                    }
                }

                _spawnedEdges = edges.ToArray();

                //_spawnedEdges = RemoveDuplicatesEdges(edges.ToArray());

                //foreach (var edge in edges)
                //{
                //    DestroyImmediate(edge);
                //}

                //for (int i = 0; i < _spawnedEdges.Length; i++)
                //{
                //    Instantiate(_spawnedEdges[i]);
                //}
            }
            else
            {
                ResetPoints();
                ResetEdges();
            }
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
        }

        private void OnDestroy()
        {
            DespawnEdges();
            DespawnPoints();
        }
    }
}