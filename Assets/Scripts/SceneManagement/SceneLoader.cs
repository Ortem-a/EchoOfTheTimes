using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        public SceneGroup[] SceneGroups;

        private float _targetProgress;
        private bool _isLoading;
        private int _lastLoadedGroupIndex;

        public readonly SceneGroupManager Manager = new SceneGroupManager();

        [Inject]
        public void Construct()
        {
            Manager.OnSceneLoaded += sceneName => Debug.Log($"Loaded: '{sceneName}'");
            Manager.OnSceneUnloaded += sceneName => Debug.Log($"Unloaded: '{sceneName}'");
            Manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");

            if (GroupToLoad < 0 || GroupToLoad >= SceneGroups.Length)
            {
                Debug.LogError($"Incorrect group to load '{GroupToLoad}'! You have only 0...{SceneGroups.Length - 1}");
                return;
            }

            Task task = LoadSceneGroupAsync(GroupToLoad);
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

            if (index < 0 || index >= SceneGroups.Length)
            {
                Debug.LogError($"Invalid scene group index: [{index}]");
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

            _lastLoadedGroupIndex = index;

            EnableLoadingCanvas();

            await Manager.LoadScenesAsync(SceneGroups[index], progress);

            EnableLoadingCanvas(false);
        }

        public async Task LoadNextSceneGroupAsync()
        {
            await LoadSceneGroupAsync(_lastLoadedGroupIndex + 1);
        }

        public bool HasNextLevel => _lastLoadedGroupIndex + 1 < SceneGroups.Length;

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCamera.gameObject.SetActive(enable);
            _loadingCanvas.gameObject.SetActive(enable);
        }
    }
}