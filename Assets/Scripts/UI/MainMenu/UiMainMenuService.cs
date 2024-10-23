using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(HeadPanelSuperviser))]
    public class UiMainMenuService : MonoBehaviour
    {
        public SceneLoader SceneLoader { get; private set; }
        public PersistenceService PersistenceService { get; private set; }

        [field: SerializeField]
        public GameObject ChaptersPanel { get; private set; }
        [field: SerializeField]
        public GameObject ChaptersFooterPanel { get; private set; }

        private ChapterItemClickHandler _lastChapterUiItem;

        [SerializeField]
        private EventSystem _eventSystem;
        [SerializeField]
        private TMP_Text _playerTotalCollectablesLabel;
        public HeadPanelSuperviser HeadPanelSuperviser { get; private set; }
        [SerializeField]
        private Transform _levelsParentPanel;

        [SerializeField]
        private Button _toLeftButton;
        [SerializeField]
        private Button _toRightButton;

        [Inject]
        private void Construct()
        {
            SceneLoader = FindObjectOfType<SceneLoader>();
            PersistenceService = FindObjectOfType<PersistenceService>();

            HeadPanelSuperviser = GetComponent<HeadPanelSuperviser>();

            for (int i = 0; i < _levelsParentPanel.transform.childCount; i++)
            {
                _levelsParentPanel.GetChild(i).gameObject.SetActive(false);
            }

            CalculateAndShowTotalPlayerProgress();
        }

        private void Awake()
        {
            HeadPanelSuperviser.ShowHeadPanelForChapters();

            UiSwipeSnapChapter.OnChapterSwiped += ShowOrHideChpaterSwitchButtons;
        }

        private void OnDestroy()
        {
            UiSwipeSnapChapter.OnChapterSwiped -= ShowOrHideChpaterSwitchButtons;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void SetActiveUi(bool active)
        {
            _eventSystem.enabled = active;
        }

        public void ShowChaptersList()
        {
            ShowChaptersUiPanels(true);
            HeadPanelSuperviser.ShowHeadPanelForChapters();
        }

        public void ShowLevelsList(ChapterItemClickHandler chapterUiItem)
        {
            _lastChapterUiItem = chapterUiItem;

            ShowChaptersUiPanels(false);
            HeadPanelSuperviser.ShowHeadPanelForLevels();
        }

        private void ShowChaptersUiPanels(bool show)
        {
            //ChaptersPanel.SetActive(show); // Как мне не трогать определённый объект на сцене тут?
            //ChaptersFooterPanel.SetActive(show);
            //_lastChapterUiItem.LevelsPanel.transform.parent.gameObject.SetActive(!show);
            GameObject excludeObject = ChaptersPanel.transform.Find("ScrollableViewHorizontalCHAPTERS_NAMES").gameObject;

            foreach (Transform child in ChaptersPanel.transform)
            {
                if (child.gameObject == excludeObject)
                {
                    continue;
                }

                child.gameObject.SetActive(show);
            }

            ChaptersFooterPanel.SetActive(show);
            _lastChapterUiItem.LevelsPanel.transform.parent.gameObject.SetActive(!show);
        }

        private void CalculateAndShowTotalPlayerProgress()
        {
            int playerProgress = 0;

            var savedData = PersistenceService.GetData();
            foreach (var chapter in savedData)
            {
                foreach (var level in chapter.Levels)
                {
                    playerProgress += level.Collected;
                }
            }

            _playerTotalCollectablesLabel.text = playerProgress.ToString();
        }

        private void ShowOrHideChpaterSwitchButtons(int currentChapterIndex)
        {
            if (currentChapterIndex == 0)
            {
                // hide left button
                _toLeftButton.gameObject.SetActive(false);
            }
            else if (currentChapterIndex == _levelsParentPanel.transform.childCount - 1)
            {
                // hide right button
                _toRightButton.gameObject.SetActive(false);
            }
            else
            {
                _toLeftButton.gameObject.SetActive(true);
                _toRightButton.gameObject.SetActive(true);
            }
        }
    }
}