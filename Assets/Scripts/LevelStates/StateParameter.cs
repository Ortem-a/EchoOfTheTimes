using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class StateParameter
    {
        public int StateId;
        public Transform Target;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 LocalScale;

        [System.NonSerialized]
        private int _defaultCompleteCounter = 0;
        [System.NonSerialized]
        private int _specialCompleteCounter = 0;
        [System.NonSerialized]
        private int _completeChecker = 3;
        [System.NonSerialized]
        private TweenCallback _onComplete;

        [System.NonSerialized]
        private float _timeToChangeState_sec;

        public void AcceptState(StateParameter specialParameter = null, bool isDebug = false, TweenCallback onComplete = null,
            float timeToChangeState_sec = 0f)
        {
            _onComplete = onComplete;

            _timeToChangeState_sec = timeToChangeState_sec;

            if (specialParameter != null)
            {
                _specialCompleteCounter = 0;
                SpecialBehaiour(specialParameter, isDebug);
            }
            else
            {
                _defaultCompleteCounter = 0;
                DefaultBehaviour(isDebug);
            }
        }

        private void DefaultBehaviour(bool isDebug)
        {
            if (!isDebug)
            {
                Target.DOMove(Position, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
                Target.DORotate(Rotation, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
                Target.DOScale(LocalScale, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
            }
            else
            {
                Target.SetPositionAndRotation(Position, Quaternion.Euler(Rotation));
                Target.localScale = LocalScale;
            }
        }

        private void SpecialBehaiour(StateParameter stateParameter, bool isDebug)
        {
            if (!isDebug)
            {
                stateParameter.Target.DOMove(stateParameter.Position, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
                stateParameter.Target.DORotate(stateParameter.Rotation, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
                stateParameter.Target.DOScale(stateParameter.LocalScale, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
            }
            else
            {
                stateParameter.Target.SetPositionAndRotation(stateParameter.Position, Quaternion.Euler(stateParameter.Rotation));
                stateParameter.Target.localScale = stateParameter.LocalScale;
            }
        }

        private void OnCompleteDefaultTransformation()
        {
            _defaultCompleteCounter++;

            if (_defaultCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }

        private void OnCompleteSpecialTransformation()
        {
            _specialCompleteCounter++;

            if (_specialCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }
    }
}