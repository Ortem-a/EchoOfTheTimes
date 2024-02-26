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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                //if (TryGetNodeByClick(Input.mousePosition, out Vertex node))
                //{
                //    MoveTo(node.transform.position);
                //}

                _path = _graph.GetPathBFS(source, destination);
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