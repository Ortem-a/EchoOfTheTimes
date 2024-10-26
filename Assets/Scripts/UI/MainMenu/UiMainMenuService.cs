using DG.Tweening;
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
        [SerializeField]
        private TMP_Text _playerTotalCollectablesLabelFromLevels;
        public HeadPanelSuperviser HeadPanelSuperviser { get; private set; }
        [SerializeField]
        private Transform _levelsParentPanel;

        [SerializeField]
        private Button _toLeftButton;
        [SerializeField]
        private Button _toRightButton;


        [Header("Элементы для пропадания при тапе в главу")]
        [SerializeField] private RectTransform soundBotton;
        [SerializeField] private RectTransform aboutUsBotton;
        [SerializeField] private RectTransform counterPanel;
        [SerializeField] private RectTransform chaptersNamesScrollable;
        [SerializeField] private RectTransform chaptersProgressScrollable;
        [SerializeField] private RectTransform circlesContainerPanel;

        [SerializeField] private Ease scaleEase = Ease.InOutQuad;

        [SerializeField] private float scaleDurationN = 0.2f;
        [SerializeField] private float targetScaleM = 1.1f;
        [SerializeField] private float targetScale = 0.5f;

        [Header("Панельки - Хаски")]
        [SerializeField] private RectTransform chaptersPanel;
        [SerializeField] private RectTransform levelsPanel;
        [SerializeField] private float durationTransitionBeetweenPanels = 1f;

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
            // ShowChaptersUiPanels(true);
            HeadPanelSuperviser.ShowHeadPanelForChapters();
        }

        public void ShowLevelsList(bool show)
        {
            // Включаем нужное меню уровней
            _lastChapterUiItem.LevelsPanel.transform.parent.gameObject.SetActive(show);

            // Сдвигаем панельки
            MovePanels(true);
        }

        private void MovePanels(bool isToLevels)
        {
            if (isToLevels)
            {
                RectTransform parentCanvas = chaptersPanel.parent.GetComponent<RectTransform>();
                float canvasWidth = parentCanvas.rect.width;

                float delay = 0.3f; // Задержка в 0.3 секунды

                // Анимация для первой панели (уходит влево за экран)
                DOTween.To(() => chaptersPanel.offsetMin, x => chaptersPanel.offsetMin = x, new Vector2(-canvasWidth, chaptersPanel.offsetMin.y), durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => chaptersPanel.offsetMax, x => chaptersPanel.offsetMax = x, new Vector2(-canvasWidth, chaptersPanel.offsetMax.y), durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                // Анимация для второй панели (становится на место первой панели)
                DOTween.To(() => levelsPanel.offsetMin, x => levelsPanel.offsetMin = x, Vector2.zero, durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => levelsPanel.offsetMax, x => levelsPanel.offsetMax = x, Vector2.zero, durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => SetActiveUi(true)); // Вызов SetActiveUi(true) после завершения анимации
            }
        }


        public void HideElementsOfChapterMenu(ChapterItemClickHandler chapterUiItem)
        {
            _lastChapterUiItem = chapterUiItem;

            EnsureCanvasGroup(soundBotton.gameObject);
            EnsureCanvasGroup(aboutUsBotton.gameObject);
            EnsureCanvasGroup(counterPanel.gameObject);
            EnsureCanvasGroup(chaptersNamesScrollable.gameObject);
            EnsureCanvasGroup(chaptersProgressScrollable.gameObject);
            EnsureCanvasGroup(circlesContainerPanel.gameObject);
            EnsureCanvasGroup(_toLeftButton.gameObject);
            EnsureCanvasGroup(_toRightButton.gameObject);

            Sequence hideSequence = DOTween.Sequence();

            // Уменьшаем до определённого размера + уменьшаем прозрачность до 0 за время M
            hideSequence.Append(aboutUsBotton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => aboutUsBotton.GetComponent<CanvasGroup>().alpha,
                             x => aboutUsBotton.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(soundBotton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => soundBotton.GetComponent<CanvasGroup>().alpha,
                             x => soundBotton.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(counterPanel.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => counterPanel.GetComponent<CanvasGroup>().alpha,
                             x => counterPanel.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(chaptersNamesScrollable.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => chaptersNamesScrollable.GetComponent<CanvasGroup>().alpha,
                             x => chaptersNamesScrollable.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(chaptersProgressScrollable.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => chaptersProgressScrollable.GetComponent<CanvasGroup>().alpha,
                             x => chaptersProgressScrollable.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(circlesContainerPanel.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => circlesContainerPanel.GetComponent<CanvasGroup>().alpha,
                             x => circlesContainerPanel.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(_toLeftButton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => _toLeftButton.GetComponent<CanvasGroup>().alpha,
                             x => _toLeftButton.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .Join(_toRightButton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
            .Join(DOTween.To(() => _toRightButton.GetComponent<CanvasGroup>().alpha,
                             x => _toRightButton.GetComponent<CanvasGroup>().alpha = x,
                             0, scaleDurationN).SetEase(scaleEase))
            .OnComplete(() =>
            {
                aboutUsBotton.gameObject.SetActive(false);
                chaptersNamesScrollable.gameObject.SetActive(false);
                chaptersProgressScrollable.gameObject.SetActive(false);
                circlesContainerPanel.gameObject.SetActive(false);
                _toLeftButton.gameObject.SetActive(false);
                _toRightButton.gameObject.SetActive(false);
                // Запускаем логику показа уровней после завершения логики перехода ОТ главы
                ShowLevelsList(true);
                // HeadPanelSuperviser.ShowHeadPanelForLevels();
                // ShowLevelsList(chapterUiItem);
            });
        }

        private void EnsureCanvasGroup(GameObject obj)
        {
            if (!obj.GetComponent<CanvasGroup>())
            {
                obj.AddComponent<CanvasGroup>();
            }
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
            _playerTotalCollectablesLabelFromLevels.text = playerProgress.ToString();
        }

        private void ShowOrHideChpaterSwitchButtons(int currentChapterIndex)
        {
            ExcludeFromHide toLeftExclude = _toLeftButton.GetComponent<ExcludeFromHide>();
            ExcludeFromHide toRightExclude = _toRightButton.GetComponent<ExcludeFromHide>();

            if (currentChapterIndex == 0)
            {
                // hide left button
                _toLeftButton.gameObject.SetActive(false);
                toLeftExclude.boundaryValue = true;
            }
            else if (currentChapterIndex == _levelsParentPanel.transform.childCount - 1)
            {
                // hide right button
                _toRightButton.gameObject.SetActive(false);
                toRightExclude.boundaryValue = true;
            }
            else
            {
                _toLeftButton.gameObject.SetActive(true);
                _toRightButton.gameObject.SetActive(true);

                toLeftExclude.boundaryValue = false;
                toRightExclude.boundaryValue = false;
            }
        }
    }
}