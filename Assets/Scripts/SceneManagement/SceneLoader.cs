using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public int GroupToLoad;

        [SerializeField]
        private Image _loadingBar;
        [SerializeField]
        private float _fillSpeed = 0.5f;
        [SerializeField]
        private Canvas _loadingCanvas;
        [SerializeField]
        private Camera _loadingCamera;

        [SerializeField]
        public SceneGroup[] _sceneGroups;

        private float _targetProgress;
        private bool _isLoading;

        public readonly SceneGroupManager Manager = new SceneGroupManager();

        private void Awake()
        {
            Manager.OnSceneLoaded += sceneName => Debug.Log($"Loaded: '{sceneName}'");
            Manager.OnSceneUnloaded += sceneName => Debug.Log($"Unloaded: '{sceneName}'");
            Manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }

        private async void Start()
        {
            if (GroupToLoad < 0 || GroupToLoad >= _sceneGroups.Length)
            {
                Debug.LogError($"Incorrect group to load '{GroupToLoad}'! You have only 0...{_sceneGroups.Length - 1}");
                return;
            }

            await LoadSceneGroupAsync(GroupToLoad);
        }

        private void Update()
        {
            if (!_isLoading) return;

            float currentFillAmount = _loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            float dynamicFillSpeed = progressDifference * _fillSpeed;

            _loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadSceneGroupAsync(int index)
        {
            _loadingBar.fillAmount = 0f;
            _targetProgress = 1f;

            if (index < 0 || index >= _sceneGroups.Length)
            {
                Debug.LogError($"Invalid scene group index: [{index}]");
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

            EnableLoadingCanvas();

            await Manager.LoadScenesAsync(_sceneGroups[index], progress);

            EnableLoadingCanvas(false);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCamera.gameObject.SetActive(enable);
            _loadingCanvas.gameObject.SetActive(enable);
        }
    }
}