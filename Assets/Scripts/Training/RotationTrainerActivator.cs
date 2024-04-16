using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    [RequireComponent(typeof(BoxCollider))]
    public class RotationTrainerActivator : MonoBehaviour
    {
        public InputMediator UserInputHandler;

        public Artifact Artifact;

        public GameObject PointerPrefab;
        public Transform SpawnPoint;

        private Transform _pointer;
        private bool _isShowed = false;
        private BoxCollider _collider;


        public RefinedOrbitCamera_PASHA ROC_PASHA;


        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;

            Artifact.gameObject.SetActive(false);

            ROC_PASHA.CanRotateCamera = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isShowed)
            {
                if (other.TryGetComponent(out Player player))
                {
                    Show();
                    _isShowed = true;
                }
            }
        }

        //private void OnTriggerExit(Collider other)
        //{
        //    if (_isShowed)
        //    {
        //        Hide();
        //    }
        //}

        public void Show()
        {
            Artifact.gameObject.SetActive(true);
            Artifact.Enable();

            _pointer = Instantiate(PointerPrefab, SpawnPoint).transform;

            _pointer.DOLocalMoveX(100f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);

            ROC_PASHA.CanRotateCamera = true;
        }

        public void Hide()
        {
            _pointer.DOScale(0f, 0.5f)
                .OnComplete(() =>
                {
                    _pointer.DOKill();
                    Destroy(gameObject);
                });
        }
    }
}