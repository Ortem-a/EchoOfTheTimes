using EchoOfTheTimes.Core;
using EchoOfTheTimes.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.Movement
{
    [RequireComponent(typeof(MonoBehaviourTimer))]
    public class Input2DIndicator : InputIndicationAnimator
    {
        [SerializeField]
        private Transform _parent;
        private bool _isFollow = false;
        private Transform _target;
        private Camera _camera;

        [SerializeField]
        private Color _defaultColor;

        private MonoBehaviourTimer _timer;

        protected override void Awake()
        {
            spawnedIndicator = Instantiate(inputIndicatorSettings.Indicator2DPrefab, Vector3.zero, Quaternion.identity, _parent);
            spawnedIndicator.SetActive(false);
            //spawnedIndicator.transform.localScale = Vector3.zero;

            _camera = Camera.main;

            spawnedIndicator.GetComponent<Image>().color = _defaultColor;

            _timer = GetComponent<MonoBehaviourTimer>();
        }

        public void ShowIndicator(Vertex at) => SpawnIndicator(at.transform);

        private void SpawnIndicator(Transform at)
        {
            _timer.Stop();

            spawnedIndicator.SetActive(true);
            _isFollow = true;
            _target = at.GetComponentInChildren<IndicationPlaceholder>().transform;

            _timer.Run(inputIndicatorSettings.IndicatorDuration2D_sec, () =>
            {
                spawnedIndicator.SetActive(false);
                _isFollow = false;
                _target = null;
            });
        }

        //private void SpawnIndicator(Transform at)
        //{
        //    spawnedIndicator.SetActive(true);
        //    _isFollow = true;
        //    //_target = at;
        //    _target = at.GetComponentInChildren<IndicationPlaceholder>().transform;

        //    spawnedIndicator.transform.DOScale(1, inputIndicatorSettings.IndicatorDuration2D_sec)
        //        .OnComplete(() =>
        //        {
        //            spawnedIndicator.transform.DOScale(0, inputIndicatorSettings.IndicatorDuration2D_sec)
        //            .OnComplete(() =>
        //            {
        //                spawnedIndicator.SetActive(false);
        //                _isFollow = false;
        //                _target = null;
        //            });
        //        });
        //}

        private void LateUpdate()
        {
            if (_isFollow)
            {
                spawnedIndicator.transform.position = _camera.WorldToScreenPoint(_target.transform.position);
            }
        }
    }
}