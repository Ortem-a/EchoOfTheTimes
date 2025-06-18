using Systems.Movement;
using Systems.Settings;
using Systems.Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Systems.Inputs
{
    [RequireComponent(typeof(MonoBehaviourTimer))]
    public class Input2DIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform _parent;
        private bool _isFollow = false;
        private Transform _target;
        private Camera _camera;

        [SerializeField]
        private Color _defaultColor;

        private MonoBehaviourTimer _timer;

        private GameObject _spawnedIndicator;
        private InputIndicatorSettingsScriptableObject _inputIndicatorSettings;

        [Inject]
        private void Construct(InputIndicatorSettingsScriptableObject inputIndicatorSettings)
        {
            _inputIndicatorSettings = inputIndicatorSettings;

            _camera = Camera.main;
            _timer = GetComponent<MonoBehaviourTimer>();
        }

        public void ShowIndicator(Vertex at)
        {
            if (_spawnedIndicator != null)
            {
                Destroy(_spawnedIndicator);
            }

            _spawnedIndicator = Instantiate(_inputIndicatorSettings.Indicator2DPrefab, Vector3.zero, Quaternion.identity, _parent);
            _spawnedIndicator.SetActive(false);
            _spawnedIndicator.GetComponent<Image>().color = _defaultColor;

            SpawnIndicator(at.transform);
        }

        private void SpawnIndicator(Transform at)
        {
            _timer.Stop();

            _spawnedIndicator.SetActive(true);
            _isFollow = true;
            _target = at;

            _timer.Run(_inputIndicatorSettings.IndicatorDuration2D_sec, () =>
            {
                _spawnedIndicator.SetActive(false);
                _isFollow = false;
                _target = null;
            });
        }

        private void LateUpdate()
        {
            if (_isFollow)
            {
                _spawnedIndicator.transform.position = _camera.WorldToScreenPoint(_target.transform.position);
            }
        }
    }
}
