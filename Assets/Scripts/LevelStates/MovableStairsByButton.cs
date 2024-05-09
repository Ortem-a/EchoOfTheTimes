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
            foreach (var stateParameter in _stairsCreator.GetStartTopPositions())
            {
                _stateService.AcceptState(stateParameter, onComplete: () => onComplete?.Invoke());
            }
        }

        private void MoveStartBottom(Action onComplete)
        {
            foreach (var stateParameter in _stairsCreator.GetStartBottomPositions())
            {
                _stateService.AcceptState(stateParameter, onComplete: () => onComplete?.Invoke());
            }
        }

        private void MoveFlatTop(Action onComplete)
        {
            foreach (var stateParameter in _stairsCreator.GetFlatTopPositions())
            {
                _stateService.AcceptState(stateParameter, onComplete: () => onComplete?.Invoke());
            }
        }

        private void MoveFlatBottom(Action onComplete)
        {
            foreach (var stateParameter in _stairsCreator.GetFlatBottomPositions())
            {
                _stateService.AcceptState(stateParameter, onComplete: () => onComplete?.Invoke());
            }
        }
    }
}