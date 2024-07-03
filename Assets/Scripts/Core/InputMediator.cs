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
        public Action OnDoubleTouched;
        public Action<float> OnSwiped;

        private Player _player;
        private GraphVisibility _graph;
        private LevelStateMachine _levelStateMachine;
        private RefinedOrbitCamera _camera;
        private InputAnimator _animator;

        private PlayerPath _playerPath;

        private void Awake()
        {
            OnTouched += HandleTouch;
            //OnSwiped += HandleSwipe;
            //OnDoubleTouched += HandleDoubleTouch;
        }

        private void OnDestroy()
        {
            OnTouched -= HandleTouch;
            //OnSwiped -= HandleSwipe;
            //OnDoubleTouched -= HandleDoubleTouch;
        }

        [Inject]
        private void Construct(GraphVisibility graph, Player player, LevelStateMachine stateMachine,
            RefinedOrbitCamera camera, InputAnimator inputAnimator, PlayerPath playerPath)
        {
            _graph = graph;
            _player = player;
            _levelStateMachine = stateMachine;
            _camera = camera;
            _animator = inputAnimator;
            _playerPath = playerPath;
        }

        private void HandleTouch(Vertex touchPosition)
        {
            _animator.Show2DIndicator(touchPosition);

            if (HasPath(touchPosition) && !_player.IsTeleportate)
            {
                _animator.ShowSuccessIndicator(touchPosition);

                _player.Stop(() => CreatePathAndMove(touchPosition));
            }
            else
            {
                _animator.ShowErrorIndicator(touchPosition);
            }
        }

        //private void HandleDoubleTouch()
        //{
        //    _camera.AutoRotateCameraAfterDoubleEmptyClick();
        //}

        //private void HandleSwipe(float swipeX)
        //{
        //    _camera.RotateCamera(swipeX);
        //}

        private bool HasPath(Vertex destination)
        {
            var path = _graph.GetPathBFS(_player.Position, destination);

            if (path.Count != 0) return true;

            return false;
        }

        private void CreatePathAndMove(Vertex destination)
        {
            List<Vertex> path = _graph.GetPathBFS((_player.NextPosition == null ? _player.Position : _player.NextPosition), destination);

            if (path.Count != 0)
            {
                path.Reverse();

                _playerPath.SetPath(path);

                _player.MoveTo(path.ToArray());
            }
        }

        public void ChangeLevelState(int levelStateId)
        {
            if (_levelStateMachine.IsChanging || levelStateId == _levelStateMachine.GetCurrentStateId())
                return;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++
            _player.CutPath();

            if (_player.StayOnDynamic)
            {
                _player.StopAndLink(onComplete: () =>
                {
                    _levelStateMachine.ChangeState(levelStateId);
                });
            }
            else if (_player.PreviousWaypointIsDynamic)
            {
                _player.WaitUntilCompleteMove(onComplete: () =>
                {
                    _levelStateMachine.ChangeState(levelStateId);
                });
            }
            else
            {
                _levelStateMachine.ChangeState(levelStateId);
            }
            // +++++++++++++++++++++++++++++++++++++++++++++++++++

            //_player.StopAndLink(onComplete: () =>
            //{
            //    _levelStateMachine.ChangeState(levelStateId);
            //});
        }
    }
}