using EchoOfTheTimes.Commands;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class UserInputHandler : MonoBehaviour
    {
        public Action<Vector3> OnMousePressed;

        private Player _player;
        private CommandManager _commandManager;
        private GraphVisibility _graph;
        private CheckpointManager _checkpointManager;
        private LevelStateMachine _levelStateMachine;

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
            _player = GameManager.Instance.Player;
            _checkpointManager = GameManager.Instance.CheckpointManager;
            _levelStateMachine = GameManager.Instance.StateMachine;
        }

        private void HandleMousePressed(Vector3 clickPosition)
        {
            if (TryGetNearestVertex(clickPosition, out Vertex destination))
            {
                List<Vertex> path = _graph.GetPathBFS(_player.Position, destination);

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
            Debug.Log("[UserInputHandler] Go To Checkpoint");

            _checkpointManager.AcceptActiveCheckpointToScene();
        }

        public void ChangeLevelState(int levelStateId)
        {
            if (_levelStateMachine.IsChanging || levelStateId == _levelStateMachine.GetCurrentStateId()) 
                return;

            _player.Stop(onComplete: () =>
            {
                _levelStateMachine.ChangeState(levelStateId);
            });
        }
    }
}