using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.UI.MainMenu;
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

        private float levelStartTime;
        private float levelDuration;

        private float _targetProgress;
        private bool _isLoading;
        private GameLevel _currentLevel;
        //private int _lastLoadedGroupIndex = -1;

        public readonly SceneGroupManager Manager = new SceneGroupManager();

        private PersistenceService _persistenceService;

        [Inject]
        public void Construct(PersistenceService persistenceService)
        {
            _persistenceService = persistenceService;

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

            if (level.FullName != "MainMenu|0")
            {
                _persistenceService.UpdateLastLoadedLevel(level);
            }

            EnableLoadingCanvas(false);
        }

        public async Task LoadNextSceneGroupAsync()
        {
            var chapter = GameChapters.Find((ch) => ch.Title == _currentLevel.ChapterName);
            if (chapter == null) return;

            var currentLevelIndex = chapter.Levels.FindIndex((lvl) => lvl.LevelName == _currentLevel.LevelName);
            if (currentLevelIndex == -1) return;

            bool isLastLevelInChapter = currentLevelIndex == chapter.Levels.Count - 1;

            if (_persistenceService.IsLevelReplayed)
            {
                // Если это перепрохождение уровня, загружаем следующий уровень в текущей главе
                if (isLastLevelInChapter)
                {
                    var nextChapterIndex = GameChapters.IndexOf(chapter) + 1;
                    if (nextChapterIndex < GameChapters.Count)
                    {
                        await LoadSceneGroupAsync(GameChapters[nextChapterIndex].Levels[0]);
                    }
                }
                else
                {
                    await LoadSceneGroupAsync(chapter.Levels[currentLevelIndex + 1]);
                }
            }
            else
            {
                // Оригинальная логика
                bool allCollectablesCollected = _persistenceService.CheckAllCollectablesCollected(chapter);

                if (isLastLevelInChapter && !allCollectablesCollected)
                {
                    await LoadSceneGroupAsync(chapter.Levels[0]);
                }
                else if (_persistenceService.IsNextChapterUnlocked)
                {
                    var nextChapterIndex = GameChapters.IndexOf(chapter) + 1;
                    if (nextChapterIndex < GameChapters.Count)
                    {
                        await LoadSceneGroupAsync(GameChapters[nextChapterIndex].Levels[0]);
                    }
                }
                else
                {
                    await LoadSceneGroupAsync(isLastLevelInChapter ? chapter.Levels[0] : chapter.Levels[currentLevelIndex + 1]);
                }
            }
        }


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
            // Загрузить сцену главного меню
            await LoadSceneGroupAsync(GameChapters[0].Levels[0]);

            // Что-то на ней сделать: МОМЕНТАЛЬНО проскроллить до этой главы и сделать нажатие
            var statusUpdater = FindObjectOfType<ChapterStatusUpdater>();

            statusUpdater
                .GetChapterSelectorItem(_persistenceService.LastLoadedLevel)
                .SelectChapter();

            statusUpdater
                .GetChapterItem(_persistenceService.LastLoadedLevel)
                .OnPointerClickSpecial(null);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCamera.gameObject.SetActive(enable);
            _loadingCanvas.gameObject.SetActive(enable);
        }
    }
}