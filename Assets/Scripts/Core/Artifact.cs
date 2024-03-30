using DG.Tweening;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class Artifact : MonoBehaviour
    {
        public Vector3 RotationAngle;
        public float RotationDuration;
        public RotateMode RotateMode;
        public LoopType RotationLoopType;
        public Ease RotationEase;

        public float MoveYEndValue;
        public float MoveDuration;
        public LoopType MoveLoopType;
        public Ease MoveEase;

        public float DisableDuration;

        public float MoveToPlayerDuration;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                transform.DOMove(player.transform.position, MoveToPlayerDuration)
                    .OnComplete(Disable);
            }
        }

        public void Enable()
        {
            transform.DOLocalRotate(RotationAngle, RotationDuration, RotateMode)
                .SetLoops(-1, RotationLoopType)
                .SetEase(RotationEase);

            transform.DOLocalMoveY(MoveYEndValue, MoveDuration)
                .SetLoops(-1, MoveLoopType)
                .SetEase(MoveEase);
        }

        public void Disable()
        {
            transform.DOScale(0f, DisableDuration)
                .OnComplete(() =>
                {
                    transform.DOKill();

                    Destroy(gameObject);
                });
        }
    }
}