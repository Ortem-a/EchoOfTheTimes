using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class InputAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _2dIndicator;

        private GameObject _spherePrefab;
        private GameObject _spawnedSphere;

        private Renderer _renderer;

        private Color _defaultSphere;
        private Color _splashSphere;
        private Color _errorSphere;
        private Color _errorSplashSphere;

        private float _defaultRadius;
        private float _maxRadius;

        private float _3dIndicatorDuration_sec;
        private float _3dIndicatorColorDuration_sec;

        [Inject]
        private void Construct(InputIndicatorSettingsScriptableObject inputIndicatorSettings)
        {
            _spherePrefab = inputIndicatorSettings.SpherePrefab;
            _defaultSphere = inputIndicatorSettings.DefaultSphere;
            _splashSphere = inputIndicatorSettings.SplashSphere;
            _errorSphere = inputIndicatorSettings.ErrorSphere;
            _errorSplashSphere = inputIndicatorSettings.ErrorSplashSphere;
            _defaultRadius = inputIndicatorSettings.DefaultRadius;
            _maxRadius = inputIndicatorSettings.MaxRadius;
            _3dIndicatorDuration_sec = inputIndicatorSettings.IndicatorDuration3D_sec;
            _3dIndicatorColorDuration_sec = inputIndicatorSettings.IndicatorColorDuration3D_sec;
        }

        private void Awake()
        {
            _spawnedSphere = Instantiate(_spherePrefab, Vector3.zero, Quaternion.identity, transform);
            _spawnedSphere.SetActive(false);

            _renderer = _spawnedSphere.GetComponent<Renderer>();
            _defaultSphere = _renderer.material.color;

            _2dIndicator.SetActive(false);
            _2dIndicator.transform.DOScale(0, 0f);
        }

        public void ShowSuccessIndicator(Vertex at) => SpawnSphere(at.transform, _defaultSphere, _splashSphere);

        public void ShowErrorIndicator(Vertex at) => SpawnSphere(at.transform, _errorSphere, _errorSplashSphere);

        private void SpawnSphere(Transform at, Color defaultColor, Color splash)
        {
            _spawnedSphere.SetActive(false);
            _renderer.material.color = defaultColor;
            _spawnedSphere.transform.localScale = Vector3.one * _defaultRadius;

            Vector3 pos = at.GetComponentInChildren<IndicationPlaceholder>().transform.position;

            _spawnedSphere.transform.localPosition = pos;
            _spawnedSphere.SetActive(true);

            _spawnedSphere.transform.DOScale(_maxRadius, _3dIndicatorDuration_sec)
                .OnComplete(() =>
                {
                    _renderer.material.DOColor(splash, _3dIndicatorColorDuration_sec);

                    _spawnedSphere.transform.DOScale(_defaultRadius, _3dIndicatorDuration_sec)
                        .OnComplete(() =>
                        {
                            _spawnedSphere.SetActive(false);
                            _renderer.material.color = defaultColor;
                        });
                });
        }

        public void Show2DIndicator(Vertex at) => Spawn2DIndicator(at.transform);

        private bool _isFollow = false;
        private Transform _target;
        private void Spawn2DIndicator(Transform at)
        {
            _2dIndicator.SetActive(true);
            _isFollow = true;
            _target = at;

            _2dIndicator.transform.DOScale(1, 0.5f)
                .OnComplete(() =>
                {
                    at.DOScale(0, 0.5f)
                    .OnComplete(() => 
                    { 
                        _2dIndicator.SetActive(false);
                        _isFollow = false;
                        _target = null;
                    });
                });
        }

        private void Update()
        {
            if (_isFollow)
            {
                _2dIndicator.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position);
            }
        }
    }
}