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
        private GraphVisibility _graph;
        private CheckpointManager _checkpointManager;

        private void Awake()
        {
            OnMousePressed += HandleMousePressed;
        }

        private void OnDestroy()
        {
            OnMousePressed -= HandleMousePressed;
        }

        public void Initialize()
        {
            _graph = GameManager.Instance.Graph;
            _commandManager = GameManager.Instance.CommandManager;
            _target = GameManager.Instance.Player;
            _checkpointManager = GameManager.Instance.CheckpointManager;
        }

        private void HandleMousePressed(Vector3 clickPosition)
        {
            if (TryGetNearestVertex(clickPosition, out Vertex destination))
            {
                List<Vertex> path = _graph.GetPathBFS(_target.Position, destination);

                if (path.Count != 0)
                {
                    path.Reverse();

                    var commands = new List<Vector3>();
                    foreach (var v in path)
                    {
                        commands.Add(v.transform.position);
                    }
                    _commandManager.UpdateCommands(commands);
                }
            }
        }

        private bool TryGetNearestVertex(Vector3 worldPosition, out Vertex vertex)
        {
            vertex = _graph.GetNearestVertex(worldPosition);

            if (vertex != null)
                return true;

            return false;
        }

        public void GoToCheckpoint()
        {
            _target.TeleportTo(_checkpointManager.ActiveCheckpoint.transform.position);
        }
    }
}