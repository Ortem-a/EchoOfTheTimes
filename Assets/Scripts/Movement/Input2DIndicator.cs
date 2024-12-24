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
            _camera = Camera.main;
            _timer = GetComponent<MonoBehaviourTimer>();
        }

        public void ShowIndicator(Vertex at)
        {
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }

            spawnedIndicator = Instantiate(inputIndicatorSettings.Indicator2DPrefab, Vector3.zero, Quaternion.identity, _parent);
            spawnedIndicator.SetActive(false);
            spawnedIndicator.GetComponent<Image>().color = _defaultColor;

            SpawnIndicator(at.transform);
        }

        private void SpawnIndicator(Transform at)
        {
            _timer.Stop();

            spawnedIndicator.SetActive(true);
            _isFollow = true;
            _target = at.GetComponentInChildren<IndicationPlaceholder>().transform;

            float animationDuration = 0f;
            var animator = spawnedIndicator.GetComponent<Animator>();
            if (animator != null)
            {
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length > 0)
                {
                    animationDuration = clipInfo[0].clip.length; // Длительность первого клипа
                    animator.Play(clipInfo[0].clip.name); // Запуск анимации
                }
            }

            _timer.Run(animationDuration, () =>
            {
                spawnedIndicator.SetActive(false);
                _isFollow = false;
                _target = null;
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
