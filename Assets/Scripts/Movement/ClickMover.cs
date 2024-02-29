using EchoOfTheTimes.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class ClickMover : AbstractUnit
    {
        public GameObject source;
        public GameObject destination;

        [SerializeField]
        private List<Vertex> _path;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Graph _graph;

        private void Start()
        {
            MoveTo(source.transform.position);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if (TryGetNodeByClick(Input.mousePosition, out Vertex node))
                //{
                //    MoveTo(node.transform.position);
                //}

                Vector3 wp = ScreenToWorldPosition(Input.mousePosition);
                destination = _graph.GetNearestVertex(wp).gameObject;

                _path = _graph.GetPathBFS(source, destination);

                _path.Reverse();

                StartCoroutine(MoveByPath(_path));

                source = destination;
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