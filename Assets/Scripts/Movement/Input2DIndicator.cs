using DG.Tweening;
using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class Input2DIndicator : InputIndicationAnimator
    {
        [SerializeField]
        private Transform _parent;
        private bool _isFollow = false;
        private Transform _target;
        private Camera _camera;

        protected override void Awake()
        {
            spawnedIndicator = Instantiate(inputIndicatorSettings.Indicator2DPrefab, Vector3.zero, Quaternion.identity, _parent);
            spawnedIndicator.SetActive(false);
            spawnedIndicator.transform.DOScale(0, 0f);

            _camera = Camera.main;
        }

        public void ShowIndicator(Vertex at) => SpawnIndicator(at.transform);

        private void SpawnIndicator(Transform at)
        {
            spawnedIndicator.SetActive(true);
            _isFollow = true;
            _target = at;

            spawnedIndicator.transform.DOScale(1, inputIndicatorSettings.IndicatorDuration2D_sec)
                .OnComplete(() =>
                {
                    at.DOScale(0, inputIndicatorSettings.IndicatorDuration2D_sec)
                    .OnComplete(() =>
                    {
                        spawnedIndicator.SetActive(false);
                        _isFollow = false;
                        _target = null;
                    });
                });
        }

        private void LateUpdate()
        {
            if (_isFollow)
            {
                spawnedIndicator.transform.position = _camera.WorldToScreenPoint(_target.transform.position);
            }
        }
    }
}