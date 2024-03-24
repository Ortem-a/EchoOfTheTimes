using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class UserInputHandler : MonoBehaviour
    {
        //public Action<Vector3> OnMousePressed;
        public Action<Vertex> OnMousePressed;

        private Player _player;
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
            _player = GameManager.Instance.Player;
            _checkpointManager = GameManager.Instance.CheckpointManager;
            _levelStateMachine = GameManager.Instance.StateMachine;
        }

        //private void HandleMousePressed(Vector3 clickPosition)
        private void HandleMousePressed(Vertex clickPosition)
        {
            _player.Stop(() => CreatePathAndMove(clickPosition));
            //if (TryGetNearestVertexInRadius(clickPosition, _graph.MaxDistanceToNeighbourVertex, out Vertex destination))
            //{
            //    _player.Stop(() => CreatePathAndMove(destination));
            //}
        }

        private void CreatePathAndMove(Vertex destination)
        {
            List<Vertex> path = _graph.GetPathBFS(_player.Position, destination);

            if (path.Count != 0)
            {
                path.Reverse();

                var waypoints = new Vector3[path.Count];
                for (int i = 0; i < path.Count; i++)
                {
                    waypoints[i] = path[i].transform.position;
                }

                _player.MoveTo(waypoints);
            }
        }

        private bool TryGetNearestVertexInRadius(Vector3 worldPosition, float radius, out Vertex vertex)
        {
            //vertex = _graph.GetNearestVertex(worldPosition);
            vertex = _graph.GetNearestVertexInRadius(worldPosition, radius);

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

            _player.StopAndLink(onComplete: () =>
            {
                _levelStateMachine.ChangeState(levelStateId);
            });
        }
    }
}