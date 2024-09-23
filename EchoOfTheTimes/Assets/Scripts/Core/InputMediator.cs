using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    [RequireComponent(typeof(Input3DIndicator), typeof(Input2DIndicator))]
    public class InputMediator : MonoBehaviour
    {
        public Action<Vertex> OnTouched;
        public Action OnDoubleTouched;
        public Action<float> OnSwiped;

        private Player _player;
        private GraphVisibility _graph;
        private LevelStateMachine _levelStateMachine;
        private RefinedOrbitCamera _camera;
        private Input3DIndicator _3dIndicator;
        private Input2DIndicator _2dIndicator;

        private PlayerPath _playerPath;

        private void Awake()
        {
            OnTouched += HandleTouch;
        }

        private void OnDestroy()
        {
            OnTouched -= HandleTouch;
        }

        [Inject]
        private void Construct(GraphVisibility graph, Player player, LevelStateMachine stateMachine,
            RefinedOrbitCamera camera, Input3DIndicator input3DIndicator, Input2DIndicator input2DIndicator, PlayerPath playerPath)
        {
            _graph = graph;
            _player = player;
            _levelStateMachine = stateMachine;
            _camera = camera;

            _3dIndicator = input3DIndicator;
            _2dIndicator = input2DIndicator;

            _playerPath = playerPath;
        }

        public void SimulateTouch(Vertex touchPosition)
        {
            HandleTouch(touchPosition, true);
        }

        private void HandleTouch(Vertex touchPosition)
        {
            HandleTouch(touchPosition, false);
        }

        private void HandleTouch(Vertex touchPosition, bool isFictitious)
        {
            if (!isFictitious)
            {
                _2dIndicator.ShowIndicator(touchPosition);
            }

            if (HasPath(touchPosition) && !_player.IsTeleportate)
            {
                if (!isFictitious)
                {
                    _3dIndicator.ShowSuccessIndicator(touchPosition);
                }

                _player.Stop(() => CreatePathAndMove(touchPosition, isFictitious));
            }
            else
            {
                if (!isFictitious)
                {
                    _3dIndicator.ShowErrorIndicator(touchPosition);
                }
            }
        }

        private bool HasPath(Vertex destination)
        {
            var path = _graph.GetPathBFS(_player.Position, destination);

            return path.Count != 0;
        }

        private void CreatePathAndMove(Vertex destination, bool isFictitious)
        {
            List<Vertex> path = _graph.GetPathBFS((_player.NextPosition == null ? _player.Position : _player.NextPosition), destination);

            if (path.Count != 0)
            {
                path.Reverse();

                _playerPath.SetPath(path);

                _player.MoveTo(path.ToArray(), isFictitious);
            }
        }

        public void ChangeLevelState(int levelStateId)
        {
            if (_levelStateMachine.IsChanging || levelStateId == _levelStateMachine.GetCurrentStateId())
                return;

            _player.CutPath();

            if (_player.StayOnDynamic)
            {
                _player.StopAndLink(onComplete: () =>
                {
                    _levelStateMachine.ChangeState(levelStateId);
                });
            }
            /*else if (_player.PreviousWaypointIsDynamic) ��� ������
            {
                _player.WaitUntilCompleteMove(onComplete: () =>
                {
                    _levelStateMachine.ChangeState(levelStateId);
                });
            }*/
            else
            {
                _levelStateMachine.ChangeState(levelStateId);
            }
        }
    }
}
