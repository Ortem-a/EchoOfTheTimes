using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
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
        public void SetOrUpdateParams()
        {
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
#endif
    }
}