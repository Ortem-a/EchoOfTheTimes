using EchoOfTheTimes.Core;
using EchoOfTheTimes.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class ClickMover : AbstractUnit
    {
        [SerializeField]
        private List<Vertex> _path;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Graph _graph;

        [SerializeField]
        private Vertex _source;
        private Vertex _destination;

        private int _index = 0;
        private bool _isMoving;
        private int _pathLength;

        private void Start()
        {
            MoveTo(_source.transform.position);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = ScreenToWorldPosition(Input.mousePosition);
                _destination = _graph.GetNearestVertex(clickPosition);

                _path = _graph.GetPathBFS(_source, _destination);

                _path.Reverse();

                _pathLength = _path.Count;
                _isMoving = true;
                _index = 0;
                transform.LookAt(_path[0].transform.position);

                //StartCoroutine(MoveByPath(_path));

                //_source = _destination;
            }

            if (_isMoving)
            {
                if (Vector3.Distance(transform.position, _path[_index].transform.position) < 0.0001f)
                {
                    if (_index < _pathLength - 1)
                    {
                        _index++;
                        transform.LookAt(_path[_index].transform.position);
                    }
                    else
                    {
                        _isMoving = false;
                        _index = 0;
                        _source = _destination;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _path[_index].transform.position, Speed * Time.deltaTime);
                }
            }
        }

        private IEnumerator MoveByPath(List<Vertex> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                yield return new WaitForSeconds(0.5f);

                MoveTo(path[i].transform.position);

                if (i + 1 < path.Count)
                {
                    transform.LookAt(path[i + 1].transform.position);
                }
            }
        }

        public Vector3 ScreenToWorldPosition(Vector3 screenPosition)
        {
            Vector3 worldPosition = Vector3.zero;
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
            {
                worldPosition = hitData.point;
            }

            return worldPosition;
        }

        public bool TryGetNodeByClick(Vector3 screenPosition, out Vertex node)
        {
            node = null;
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
            {
                if (_graph.TryGetNode(hitData.collider.gameObject.name, out node))
                {
                    return true;
                }
            }

            return false;
        }
    }
}