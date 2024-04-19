using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    public class InputMediator : MonoBehaviour
    {
        public Action<Vertex> OnTouched;
        public Action<float> OnSwipe;

        private Player _player;
        private GraphVisibility _graph;
        private CheckpointManager _checkpointManager;
        private LevelStateMachine _levelStateMachine;
        private RefinedOrbitCamera _camera;

        private void Awake()
        {
            OnTouched += HandleTouch;
            OnSwipe += HandleSwipe;
        }

        private void OnDestroy()
        {
            OnTouched -= HandleTouch;
            OnSwipe -= HandleSwipe;
        }

        [Inject]
        private void Construct(GraphVisibility graph, Player player, CheckpointManager checkpointManager, LevelStateMachine stateMachine, RefinedOrbitCamera camera)
        {
            _graph = graph;
            _player = player;
            _checkpointManager = checkpointManager;
            _levelStateMachine = stateMachine;
            _camera = camera;
        }

        private void HandleTouch(Vertex touchPosition)
        {
            _player.Stop(() => CreatePathAndMove(touchPosition));
        }

        private void HandleSwipe(float deltaX)
        {
            _camera.RotateCamera(deltaX);
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