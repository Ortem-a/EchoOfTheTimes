using System;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class MovableByButton : MonoBehaviour
    {
        [SerializeField]
        private StateParameter _parameter;

        public void Move(Action onComplete)
        {
            _parameter.AcceptState(onComplete: () => onComplete());
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