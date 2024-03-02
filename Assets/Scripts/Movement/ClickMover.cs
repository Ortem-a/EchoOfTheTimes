using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class ClickMover : MonoBehaviour
    {
        private IUnit _target;

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

        private void Awake()
        {
            _target = GetComponent<IUnit>();
        }

        private void Start()
        {
            _target.TeleportTo(_source.transform.position);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = ScreenToWorldPosition(Input.mousePosition);

                if (TryGetNearestVertex(clickPosition, out _destination))
                {
                    _path = _graph.GetPathBFS(_source, _destination);
                    _path.Reverse();

                    _target.MoveTo(_path[0].transform.position);

                    //_pathLength = _path.Count;
                    //_isMoving = true;
                    //_index = 0;
                    //transform.LookAt(_path[0].transform.position);

                    //StartCoroutine(MoveByPath(_path));

                    _source = _destination;
                }
            }

            if (_isMoving)
            {
                if (Vector3.Distance(transform.position, _path[_index].transform.position) < Mathf.Epsilon)
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
                    transform.position = Vector3.MoveTowards(transform.position, _path[_index].transform.position, _target.Speed * Time.deltaTime);
                }
            }
        }

        private IEnumerator MoveByPath(List<Vertex> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                yield return new WaitForSeconds(0.5f);

                _target.TeleportTo(path[i].transform.position);

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

        public bool TryGetNearestVertex(Vector3 worldPosition, out Vertex vertex)
        {
            vertex = _graph.GetNearestVertex(worldPosition);

            if (vertex != null)
                return true;

            return false;
        }
    }
}