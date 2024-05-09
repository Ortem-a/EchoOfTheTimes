using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class MovableByButton : MonoBehaviour
    {
        [SerializeField]
        private StateParameter _parameter;

        private StateService _stateService;

        [Inject]
        private void Construct(StateService stateService)
        {
            _stateService = stateService;
        }

        public virtual void Move(Action onComplete)
        {
            _stateService.AcceptState(_parameter, onComplete: () => onComplete?.Invoke());
        }

        public void SetOrUpdateParams()
        {
            var newStateParam = new StateParameter
            {
                StateId = 0,
                Target = transform,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                LocalScale = transform.localScale,
            };

            _parameter = newStateParam;
        }

        public void TransformObjectByParams()
        {
            transform.SetPositionAndRotation(_parameter.Position, Quaternion.Euler(_parameter.Rotation));
            transform.localScale = _parameter.LocalScale;
        }
    }
}