using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public PlayerPath PlayerPath;

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
        private void Construct(GraphVisibility graph, Player player, LevelStateMachine stateMachine, RefinedOrbitCamera camera)
        {
            _graph = graph;
            _player = player;
            _levelStateMachine = stateMachine;
            _camera = camera;
        }

        private void HandleTouch(Vertex touchPosition)
        {
            if (HasPath(touchPosition)) 
            {
                _player.Stop(() => CreatePathAndMove(touchPosition));
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

                PlayerPath.SetPath(path);

                _player.MoveTo(path.ToArray());
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