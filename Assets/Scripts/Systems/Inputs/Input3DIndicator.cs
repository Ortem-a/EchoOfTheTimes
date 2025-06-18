using DG.Tweening;
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

        private GameObject _spawnedIndicator;
        private InputIndicatorSettingsScriptableObject _inputIndicatorSettings;

        [Inject]
        private void Construct(InputIndicatorSettingsScriptableObject inputIndicatorSettings)
        {
            _inputIndicatorSettings = inputIndicatorSettings;
        }

        public void ShowSuccessIndicator(Vertex at) => SpawnSphere(at.transform, _defaultSphere, _splashSphere);

        public void ShowErrorIndicator(Vertex at) => SpawnSphere(at.transform, _defaultErrorSplashSphere, _errorSplashSphere);

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
    }
}
