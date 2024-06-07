using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class StateParameter : IStateParameter
    {
        [field: SerializeField]
        public int StateId { get; set; }
        [field: SerializeField]
        public Transform Target { get; set; }
        [field: SerializeField]
        public Vector3 Position { get; set; }
        [field: SerializeField]
        public Vector3 Rotation { get; set; }
        [field: SerializeField]
        public Vector3 LocalScale { get; set; }

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
                Target.DOMove(Position, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteTransformation());
                Target.DORotate(Rotation, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteTransformation());
                Target.DOScale(LocalScale, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteTransformation());
            }
            else
            {
                Target.SetPositionAndRotation(Position, Quaternion.Euler(Rotation));
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

    public interface IStateParameter
    {
        public int StateId { get; set; }
        public Transform Target {get;set;}
        public Vector3 Position {get;set;}
        public Vector3 Rotation {get;set;}
        public Vector3 LocalScale {get;set;}

        public void AcceptState(bool isDebug = false, TweenCallback onComplete = null, float timeToChangeState_sec = 0f);
    }
}