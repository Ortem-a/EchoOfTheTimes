using EchoOfTheTimes.Utils;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(MovableByButtonGizmosDrawer))]
    public class MovableByButton : MonoBehaviour
    {
        public bool IsLocalSpace;

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

#if UNITY_EDITOR
        [SerializeField]
        private StateParameter _defaultPosition;

        public StateParameter Parameter => _parameter;
        public StateParameter GetDefaultPosition()
        {
            return _defaultPosition ?? new StateParameter
            {
                StateId = 0,
                Target = transform,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                LocalScale = transform.localScale,
                IsLocalSpace = IsLocalSpace
            };
        }

        public void SetOrUpdateParams()
        {
            SetDefaultPosition();

            Vector3 newPosition = transform.position;
            Vector3 newRotation = transform.rotation.eulerAngles;
            if (IsLocalSpace)
            {
                newPosition = transform.localPosition;
                newRotation = transform.localRotation.eulerAngles;
            }

            var newStateParam = new StateParameter
            {
                StateId = 0,
                Target = transform,
                Position = newPosition,
                Rotation = newRotation,
                LocalScale = transform.localScale,
                IsLocalSpace = IsLocalSpace
            };

            _parameter = newStateParam;
        }

        public void TransformObjectByParams()
        {
            SetDefaultPosition();

            if (IsLocalSpace)
            {
                transform.SetLocalPositionAndRotation(_parameter.Position, Quaternion.Euler(_parameter.Rotation));
            }
            else
            {
                transform.SetPositionAndRotation(_parameter.Position, Quaternion.Euler(_parameter.Rotation));
            }
            transform.localScale = _parameter.LocalScale;
        }

        private void SetDefaultPosition()
        {
            _defaultPosition = new StateParameter
            {
                StateId = 0,
                Target = transform,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                LocalScale = transform.localScale,
                IsLocalSpace = IsLocalSpace
            };
        }

        public void TransformToDefaultPosition()
        {
            transform.SetPositionAndRotation(_defaultPosition.Position, Quaternion.Euler(_defaultPosition.Rotation));
            transform.localScale = _defaultPosition.LocalScale;
        }
#endif
    }
}