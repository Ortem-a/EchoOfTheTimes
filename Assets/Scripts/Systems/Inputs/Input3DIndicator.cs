using DG.Tweening;
using DG.Tweening.Core.Easing;
using Systems.Movement;
using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.Inputs
{
    public class Input3DIndicator : MonoBehaviour
    {
        [SerializeField]
        private Color _defaultSphere = Color.black;
        [SerializeField]
        private Color _splashSphere = Color.white;
        [SerializeField]
        private Color _defaultErrorSplashSphere = Color.white;
        [SerializeField]
        private Color _errorSplashSphere = Color.red;

        private Sequence _animation;
        private GameObject _spawnedIndicator;
        private InputIndicatorSettingsScriptableObject _inputIndicatorSettings;

        private Material _indicationMaterial;

        [Inject]
        private void Construct(InputIndicatorSettingsScriptableObject inputIndicatorSettings)
        {
            _inputIndicatorSettings = inputIndicatorSettings;
        }

        private void Awake()
        {
#warning подумать свежим мозгом
            // хочу переиспользовать 1 объект для индикации
            // создать 1 раз анимации для
            // - успешного нажатия
            // - нажатия с ошибкой
            // при любом нажатии запускается уже созданная и подготовленная 
            // анимация для ранее созданного объекта
            _spawnedIndicator = Instantiate(
                _inputIndicatorSettings.Indicator3DPrefab,
                Vector3.zero,
                Quaternion.identity,
                transform);

            _indicationMaterial = _spawnedIndicator.GetComponent<Renderer>().sharedMaterial;

            _spawnedIndicator.SetActive(false);
            _indicationMaterial.color = _defaultSphere;
            _spawnedIndicator.transform.localScale = Vector3.one * _inputIndicatorSettings.DefaultRadius;

            _animation = DOTween.Sequence()
                .OnStart(() => _spawnedIndicator.SetActive(true))
                .OnComplete(() => _spawnedIndicator.SetActive(false))
                .Append(_spawnedIndicator.transform
                    .DOScale(_inputIndicatorSettings.MaxRadius, _inputIndicatorSettings.IndicatorDuration3D_sec))
                .Join(_indicationMaterial
                    .DOColor(_defaultSphere, _inputIndicatorSettings.IndicatorColorDuration3D_sec))
                .Join(_spawnedIndicator.transform
                    .DOScale(_inputIndicatorSettings.DefaultRadius, _inputIndicatorSettings.IndicatorDuration3D_sec));
        }

        public void ShowSuccessIndicator(Vertex at) => SpawnSphere(
            at.IndicationPlaceholder.transform,
            _defaultSphere,
            _splashSphere);

        public void ShowErrorIndicator(Vertex at) => SpawnSphere(
            at.IndicationPlaceholder.transform,
            _defaultErrorSplashSphere,
            _errorSplashSphere);

        private void SpawnSphere(Transform at, Color defaultColor, Color splash)
        {
            if (_spawnedIndicator != null)
            {
                Destroy(_spawnedIndicator);
            }

            _spawnedIndicator = Instantiate(_inputIndicatorSettings.Indicator3DPrefab, Vector3.zero, Quaternion.identity, transform);
            _spawnedIndicator.SetActive(false);
            var renderer = _spawnedIndicator.GetComponent<Renderer>();
            renderer.material.color = defaultColor;
            _spawnedIndicator.transform.localScale = Vector3.one * _inputIndicatorSettings.DefaultRadius;

            _spawnedIndicator.transform.localPosition = at.position;
            _spawnedIndicator.SetActive(true);

            _spawnedIndicator.transform.DOScale(_inputIndicatorSettings.MaxRadius, _inputIndicatorSettings.IndicatorDuration3D_sec)
                .OnComplete(() =>
                {
                    renderer.material.DOColor(splash, _inputIndicatorSettings.IndicatorColorDuration3D_sec);

                    _spawnedIndicator.transform.DOScale(_inputIndicatorSettings.DefaultRadius, _inputIndicatorSettings.IndicatorDuration3D_sec)
                        .OnComplete(() =>
                        {
                            _spawnedIndicator.SetActive(false);
                            renderer.material.color = defaultColor;
                            Destroy(_spawnedIndicator); // Destroy the indicator after use
                        });
                });
        }

        private void ShowIndicator(Transform at, Color defaultColor, Color splash)
        {
            if (_animation.IsPlaying())
            {
                // hide current indication
                _animation.Kill();
            }

            _spawnedIndicator.transform.SetParent(at);
            _indicationMaterial.color = defaultColor;
            _spawnedIndicator.transform.localScale = Vector3.one * _inputIndicatorSettings.DefaultRadius;
            _spawnedIndicator.transform.localPosition = at.position;
            
            _animation.Play();
        }
    }
}
