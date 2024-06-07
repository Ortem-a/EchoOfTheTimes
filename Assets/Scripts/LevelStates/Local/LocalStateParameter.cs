using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Local
{
    [System.Serializable]
    public class LocalStateParameter : IStateParameter
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

        public void AcceptState(bool isDebug = false, TweenCallback onComplete = null, float timeToChangeState_sec = 0)
        {
            _onComplete = onComplete;

            _timeToChangeState_sec = timeToChangeState_sec;

            _defaultCompleteCounter = 0;
            ExecuteLocalTransformations(isDebug);
        }

        private void ExecuteLocalTransformations(bool isDebug)
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

        protected void OnCompleteTransformation()
        {
            _defaultCompleteCounter++;

            if (_defaultCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }
    }
}