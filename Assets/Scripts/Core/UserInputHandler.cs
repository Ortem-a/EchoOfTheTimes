using EchoOfTheTimes.Commands;
using EchoOfTheTimes.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class UserInputHandler : MonoBehaviour
    {
        public Action<Vector3> OnMousePressed;

        private IUnit _target;

        private CommandManager _commandManager;

        [SerializeField]
        private List<Vertex> _path;

        [SerializeField]
        private GraphVisibility _graph;

        private void Awake()
        {
            OnMousePressed += HandleMousePressed;

            _target = GetComponent<IUnit>();
            _commandManager = GetComponent<CommandManager>();
        }

        private void OnDestroy()
        {
            OnMousePressed -= HandleMousePressed;
        }

        private void HandleMousePressed(Vector3 clickPosition)
        {
            if (TryGetNearestVertex(clickPosition, out Vertex destination))
            {
                _path = _graph.GetPathBFS(_target.Position, destination);
                _path.Reverse();

                var commands = new List<Vector3>();
                foreach (var v in _path)
                {
                    commands.Add(v.transform.position);
                }
                _commandManager.UpdateCommands(commands);
            }
        }

        private bool TryGetNearestVertex(Vector3 worldPosition, out Vertex vertex)
        {
            vertex = _graph.GetNearestVertex(worldPosition);

            if (vertex != null)
                return true;

            return false;
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
    }
}