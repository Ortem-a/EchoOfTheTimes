using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class MovableStairsByButton : MovableByButton
    {
        [SerializeField]
        private bool _flatBottom;
        [SerializeField]
        private bool _flatTop;
        [SerializeField]
        private bool _startBottom;
        [SerializeField]
        private bool _startTop;

        private StateService _stateService;

        private StairsCreator _stairsCreator;

        [Inject]
        private void Construct(StateService stateService)
        {
            _stateService = stateService;

            _stairsCreator = GetComponent<StairsCreator>();
        }

        private void Awake()
        {
            //_stairsCreator = GetComponent<StairsCreator>();
        }

        public override void Move(Action onComplete)
        {
            if (_flatBottom) MoveFlatBottom(onComplete);
            else if (_flatTop) MoveFlatTop(onComplete);
            else if (_startBottom) MoveStartBottom(onComplete);
            else if (_startTop) MoveStartTop(onComplete);
            else onComplete?.Invoke();
        }

        private void MoveStartTop(Action onComplete)
        {
            if (_stairsCreator.StartTopIds != null && _stairsCreator.StartTopIds.Count > 0)
            {
                MoveById(_stairsCreator.StartTopIds[0], onComplete);
            }
        }

        private void MoveStartBottom(Action onComplete)
        {
            if (_stairsCreator.StartBottomIds != null && _stairsCreator.StartBottomIds.Count > 0)
            {
                MoveById(_stairsCreator.StartBottomIds[0], onComplete);
            }
        }

        private void MoveFlatTop(Action onComplete)
        {
            if (_stairsCreator.FlatTopIds != null && _stairsCreator.FlatTopIds.Count > 0)
            {
                MoveById(_stairsCreator.FlatTopIds[0], onComplete);
            }
        }

        private void MoveFlatBottom(Action onComplete)
        {
            if (_stairsCreator.FlatBottomIds != null && _stairsCreator.FlatBottomIds.Count > 0)
            {
                MoveById(_stairsCreator.FlatBottomIds[0], onComplete);
            }
        }

        private void MoveById(int id, Action onComplete)
        {
            foreach (var stair in _stairsCreator.Stairs)
            {
                var stateParameter = stair.Stateable.States.Find(x => x.StateId == id);

                _stateService.AcceptState(stateParameter, onComplete: () => onComplete?.Invoke());
            }
        }
    }
}