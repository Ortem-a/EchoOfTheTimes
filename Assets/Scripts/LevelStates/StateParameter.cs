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
        private int _completeChecker = 3;
        [System.NonSerialized]
        private TweenCallback _onComplete;

        [System.NonSerialized]
        private float _timeToChangeState_sec;

        public void AcceptState(bool isDebug = false, TweenCallback onComplete = null,
            float timeToChangeState_sec = 0f)
        {
            _onComplete = onComplete;

            _timeToChangeState_sec = timeToChangeState_sec;

            _defaultCompleteCounter = 0;
            ExecuteTransformations(isDebug);
        }

        private void ExecuteTransformations(bool isDebug)
        {
            if (!isDebug)
            {
                Target.DOLocalMove(Position, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteTransformation());
                Target.DOLocalRotate(Rotation, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteTransformation());
                Target.DOScale(LocalScale, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteTransformation());
            }
            else
            {
                Target.SetLocalPositionAndRotation(Position, Quaternion.Euler(Rotation));
                Target.localScale = LocalScale;

                _onComplete?.Invoke();
            }
        }

        private void OnCompleteTransformation()
        {
            _defaultCompleteCounter++;

            if (_defaultCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }
    }
}