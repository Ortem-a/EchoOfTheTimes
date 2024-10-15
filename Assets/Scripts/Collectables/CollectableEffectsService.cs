using DG.Tweening;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    public class CollectableEffectsService : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField]
        private float _height;
        [SerializeField]
        private float _moveDuration;
        [SerializeField]
        private Ease _moveEase;

        [Header("Rotation settings")]
        [SerializeField]
        private Vector3 _rotation;
        [SerializeField]
        private float _rotationDuration;
        [SerializeField]
        private Ease _rotationEase;

        [Header("On collect settings")]
        [SerializeField]
        private float _maxHeight;
        [SerializeField]
        private float _onCollectDuration;

        private Tweener _moveTweener = null;
        private Tweener _rotationTweener = null;
        private Tweener _completeTweener = null;

        public void Idle()
        {
            _moveTweener = transform.DOLocalMoveY(_height, _moveDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(_moveEase);
            _rotationTweener = transform.DOLocalRotate(_rotation, _rotationDuration)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(_rotationEase);
        }

        public void OnCollect(Action onComplete)
        {
            _completeTweener = transform.DOMoveY(_maxHeight, _onCollectDuration)
                .OnComplete(() => onComplete?.Invoke());
        }

        private void OnDestroy()
        {
            _moveTweener?.Kill();
            _rotationTweener?.Kill();
            _completeTweener?.Kill();
        }
    }
}