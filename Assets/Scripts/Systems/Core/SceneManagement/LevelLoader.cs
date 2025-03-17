using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Core.SceneManagement
{
    public class LevelLoader : MonoBehaviour
    {
        public int ChapterIndexToLoad;
        public int LevelIndexToLoad;

        [SerializeField]
        private Image _loadingBar;
        [SerializeField]
        private float _fillSpeed = 0.5f;
        [SerializeField]
        private Canvas _loadingCanvas;
        [SerializeField]
        private Camera _loadingCamera;

        public GameChapter[] Chapters;

        private float _targetProgress;
        private bool _isLoading;
        private GameLevel _currentLevel;

        private readonly SceneLoader _loader = new SceneLoader();

        private async void Awake()
        {
            await LoadMainMenuSceneAsync();
        }

        private void Update()
        {
            if (!_isLoading) return;

            float currentFillAmount = _loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            float dynamicFillSpeed = progressDifference * _fillSpeed;

            _loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadLevelAsync(GameLevel level)
        {
            await _loader.UnloadSceneAsync();

            _currentLevel = level;

            _loadingBar.fillAmount = 0f;
            _targetProgress = 1f;

            var progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

            EnableLoadingCanvas();

            await _loader.LoadSceneAsync(level, progress);

            EnableLoadingCanvas(false);
        }

        public async Task LoadNextLevelAsync()
        {
            var chapter = Array.Find(Chapters, (chapter) => chapter.Title == _currentLevel.ChapterName);
            if (chapter == null) return;

            var currentLevelIndex = Array.FindIndex(chapter.Levels, (level) => level.LevelName == _currentLevel.LevelName);

            await LoadLevelAsync(chapter.Levels[currentLevelIndex + 1]);
        }

        public async Task LoadMainMenuSceneAsync()
        {
            await LoadLevelAsync(Chapters[0].Levels[0]);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCamera.gameObject.SetActive(enable);
            _loadingCanvas.gameObject.SetActive(enable);
        }
    }
}