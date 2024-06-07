using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates.Local
{
    public class LocalMovableByButton : MonoBehaviour
    {
        [SerializeField]
        private LocalStateParameter _parameter;

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
            var newStateParam = new LocalStateParameter
            {
                StateId = 0,
                Target = transform,
                Position = transform.localPosition,
                Rotation = transform.localRotation.eulerAngles,
                LocalScale = transform.localScale,
            };

            _parameter = newStateParam;
        }

        public void TransformObjectByParams()
        {
            transform.SetLocalPositionAndRotation(_parameter.Position, Quaternion.Euler(_parameter.Rotation));
            transform.localScale = _parameter.LocalScale;
        }
#endif
    }
}