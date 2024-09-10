using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.SceneManagement
{
    public class SceneLoader : MonoBehaviour
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

        //public SceneGroup[] SceneGroups;
        public List<GameChapter> GameChapters;

        private float _targetProgress;
        private bool _isLoading;
        private GameLevel _currentLevel;
        //private int _lastLoadedGroupIndex = -1;

        public readonly SceneGroupManager Manager = new SceneGroupManager();

        [Inject]
        public void Construct()
        {
            Manager.OnSceneLoaded += sceneName =>
            {
                Debug.Log($"Loaded: '{sceneName}'");
            };

            Manager.OnSceneUnloaded += sceneName =>
            {
                Debug.Log($"Unloaded: '{sceneName}'");
            };

            Manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");

            if (ChapterIndexToLoad < 0 || ChapterIndexToLoad >= GameChapters.Count)
            {
                Debug.LogError($"Incorrect chapter to load '{ChapterIndexToLoad}'! You have only 0...{GameChapters.Count - 1}");
                return;
            }
            if (LevelIndexToLoad < 0 || LevelIndexToLoad >= GameChapters[ChapterIndexToLoad].Levels.Count)
            {
                Debug.LogError($"Incorrect level to load '{LevelIndexToLoad}'! You have only 0...{GameChapters[ChapterIndexToLoad].Levels.Count - 1}");
                return;
            }

            Task task = LoadSceneGroupAsync(GameChapters[ChapterIndexToLoad].Levels[LevelIndexToLoad]);
        }

        private void Update()
        {
            if (!_isLoading) return;

            float currentFillAmount = _loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            float dynamicFillSpeed = progressDifference * _fillSpeed;

            _loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        //public async Task LoadSceneGroupAsync(int index)
        //{
        //    _loadingBar.fillAmount = 0f;
        //    _targetProgress = 1f;

        //    if (index < 0 || index >= GameChapters.Length)
        //    {
        //        Debug.LogError($"Invalid scene group index: [{index}]");
        //        return;
        //    }

        //    LoadingProgress progress = new LoadingProgress();
        //    progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

        //    EnableLoadingCanvas();

        //    if (_lastLoadedGroupIndex >= 0)
        //    {
        //        string previousSceneName = GameChapters[_lastLoadedGroupIndex].GroupName;
        //        Debug.Log($"Calling OnSceneUnloaded for scene: {previousSceneName}");
        //    }

        //    _lastLoadedGroupIndex = index;

        //    await Manager.LoadScenesAsync(GameChapters[index], progress);

        //    string currentSceneName = GameChapters[index].GroupName;
        //    Debug.Log($"Calling OnSceneLoaded for scene: {currentSceneName}");

        //    EnableLoadingCanvas(false);
        //}

        public async Task LoadSceneGroupAsync(GameLevel level)
        {
            _currentLevel = level;

            _loadingBar.fillAmount = 0f;
            _targetProgress = 1f;

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

            EnableLoadingCanvas();

            await Manager.LoadScenesAsync(level, progress);

            Debug.Log($"Calling OnSceneLoaded for scene: {level.LevelName}");

            EnableLoadingCanvas(false);
        }

        public async Task LoadNextSceneGroupAsync()
        {
            var chapter = GameChapters.Find((chapter) => chapter.Title == _currentLevel.ChapterName);
            if (chapter == null) return;

            var currentLevelIndex = chapter.Levels.FindIndex((level) => level.LevelName == _currentLevel.LevelName);

            await LoadSceneGroupAsync(chapter.Levels[currentLevelIndex + 1]);

            //await LoadSceneGroupAsync(_lastLoadedGroupIndex + 1);
        }

        ///public bool HasNextLevel => _lastLoadedGroupIndex + 1 < GameChapters.Length;
        public bool HasNextLevel
        {
            get
            {
                var chapter = GameChapters.Find((chapter) => chapter.Title == _currentLevel.ChapterName);
                if (chapter == null) return false;

                var currentLevelIndex = chapter.Levels.FindIndex((level) => level.LevelName == _currentLevel.LevelName);

                if (currentLevelIndex == -1) return false;

                if (currentLevelIndex < chapter.Levels.Count - 1) return true;

                return false;
            }
        }

        public async Task LoadMainMenuSceneAsync()
        {
            await LoadSceneGroupAsync(GameChapters[0].Levels[0]);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCamera.gameObject.SetActive(enable);
            _loadingCanvas.gameObject.SetActive(enable);
        }
    }
}