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
    public class UiMainMenuService : MonoBehaviour
    {
        public SceneLoader SceneLoader { get; private set; }
        public PersistenceService PersistenceService { get; private set; }

        [field: SerializeField]
        public GameObject ChaptersPanel { get; private set; }
        [field: SerializeField]
        public GameObject ChaptersFooterPanel { get; private set; }

        private ChapterItemClickHandler _lastChapterUiItem;

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private TMP_Text _playerTotalCollectablesLabel;
        [SerializeField] private TMP_Text _playerTotalCollectablesLabelFromLevels;
        [SerializeField] private Transform _levelsParentPanel;
        [SerializeField] private Button _toLeftButton;
        [SerializeField] private Button _toRightButton;


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

        [Header("Элементы для пропадания при тапе из уровня")]
        [SerializeField] private RectTransform toChaptersButton;
        [SerializeField] private float scaleDuration = 0.2f;
        [SerializeField] private float scaleMax = 1.1f;


        [Header("Панельки - Хаски")]
        [SerializeField] private RectTransform chaptersPanel;
        [SerializeField] private RectTransform levelsPanel;
        [SerializeField] private float durationTransitionBeetweenPanels = 1f;

        [Inject]
        private void Construct()
        {
            SceneLoader = FindObjectOfType<SceneLoader>();
            PersistenceService = FindObjectOfType<PersistenceService>();

            for (int i = 0; i < _levelsParentPanel.transform.childCount; i++)
            {
                _levelsParentPanel.GetChild(i).gameObject.SetActive(false);
            }

            CalculateAndShowTotalPlayerProgress();
        }

        private void Awake()
        {
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

        // Жмакнули кнопку выхода в меню глав
        public void ShowChaptersList()
        {
            // Отключаем взаимодействие UI
            SetActiveUi(false);

            // Создаем последовательность для кнопки toChaptersButton
            Sequence buttonSequence = DOTween.Sequence();
            buttonSequence.Append(toChaptersButton.DOScale(scaleMax, scaleDuration / 2))
                         .Append(toChaptersButton.DOScale(1f, scaleDuration / 2));

            // Врубаем всё что выключали на меню глав
            RestoreElementsOfChapterMenu(durationTransitionBeetweenPanels);

            // Перемещаем панельки к разделу глав
            MovePanels(false);
        }

        public void ShowLevelsList(bool show)
        {
            // Включаем нужное меню уровней
            _lastChapterUiItem.LevelsPanel.transform.parent.gameObject.SetActive(show);

            // Сдвигаем панельки
            MovePanels(true);
        }

        private void MovePanels(bool isLevels)
        {
            RectTransform parentCanvas = chaptersPanel.parent.GetComponent<RectTransform>();
            float canvasWidth = parentCanvas.rect.width;

            if (isLevels)
            {
                float delay = 0.3f; // Задержка

                // Анимация для первой панели (уходит влево за экран)
                DOTween.To(() => chaptersPanel.offsetMin, x => chaptersPanel.offsetMin = x, new Vector2(-canvasWidth, chaptersPanel.offsetMin.y), durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => chaptersPanel.offsetMax, x => chaptersPanel.offsetMax = x, new Vector2(-canvasWidth, chaptersPanel.offsetMax.y), durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                // Анимация для второй панели (вплывает справа на место первой панели)
                DOTween.To(() => levelsPanel.offsetMin, x => levelsPanel.offsetMin = x, Vector2.zero, durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => levelsPanel.offsetMax, x => levelsPanel.offsetMax = x, Vector2.zero, durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => SetActiveUi(true)); // Включаем взаимодействие после завершения анимации
            }
            else
            {
                float delay = 0.0f; // Задержка

                // Анимация для первой панели (возвращается на место)
                DOTween.To(() => chaptersPanel.offsetMin, x => chaptersPanel.offsetMin = x, Vector2.zero, durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => chaptersPanel.offsetMax, x => chaptersPanel.offsetMax = x, Vector2.zero, durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                // Анимация для второй панели (уходит вправо за экран)
                DOTween.To(() => levelsPanel.offsetMin, x => levelsPanel.offsetMin = x, new Vector2(canvasWidth, levelsPanel.offsetMin.y), durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => levelsPanel.offsetMax, x => levelsPanel.offsetMax = x, new Vector2(canvasWidth, levelsPanel.offsetMax.y), durationTransitionBeetweenPanels)
                    .SetDelay(delay)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        SetActiveUi(true);

                        if (_lastChapterUiItem != null && _lastChapterUiItem.LevelsPanel != null)
                        {
                            _lastChapterUiItem.LevelsPanel.transform.parent.gameObject.SetActive(false);
                        }
                    });
            }
        }

        public void RestoreElementsOfChapterMenu(float restoreDuration)
        {
            // Активируем все элементы перед восстановлением
            aboutUsBotton.gameObject.SetActive(true);
            soundBotton.gameObject.SetActive(true);
            counterPanel.gameObject.SetActive(true);
            chaptersNamesScrollable.gameObject.SetActive(true);
            chaptersProgressScrollable.gameObject.SetActive(true);
            circlesContainerPanel.gameObject.SetActive(true);

            ExcludeFromHide toLeftExclude = _toLeftButton.GetComponent<ExcludeFromHide>();
            ExcludeFromHide toRightExclude = _toRightButton.GetComponent<ExcludeFromHide>();

            // Активируем кнопки _toLeftButton и _toRightButton
            if (toLeftExclude == null || !toLeftExclude.boundaryValue)
                _toLeftButton.gameObject.SetActive(true);

            if (toRightExclude == null || !toRightExclude.boundaryValue)
                _toRightButton.gameObject.SetActive(true);

            // Создаем последовательность для восстановления элементов
            Sequence restoreSequence = DOTween.Sequence();

            restoreSequence.Append(aboutUsBotton.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => aboutUsBotton.GetComponent<CanvasGroup>().alpha,
                                 x => aboutUsBotton.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase))
                .Join(soundBotton.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => soundBotton.GetComponent<CanvasGroup>().alpha,
                                 x => soundBotton.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase))
                .Join(counterPanel.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => counterPanel.GetComponent<CanvasGroup>().alpha,
                                 x => counterPanel.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase))
                .Join(chaptersNamesScrollable.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => chaptersNamesScrollable.GetComponent<CanvasGroup>().alpha,
                                 x => chaptersNamesScrollable.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase))
                .Join(chaptersProgressScrollable.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => chaptersProgressScrollable.GetComponent<CanvasGroup>().alpha,
                                 x => chaptersProgressScrollable.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase))
                .Join(circlesContainerPanel.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => circlesContainerPanel.GetComponent<CanvasGroup>().alpha,
                                 x => circlesContainerPanel.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase));

            // Восстанавливаем _toLeftButton, если boundaryValue = false
            if (toLeftExclude == null || !toLeftExclude.boundaryValue)
            {
                restoreSequence.Join(_toLeftButton.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                    .Join(DOTween.To(() => _toLeftButton.GetComponent<CanvasGroup>().alpha,
                                     x => _toLeftButton.GetComponent<CanvasGroup>().alpha = x,
                                     1f, restoreDuration).SetEase(scaleEase));
            }

            // Восстанавливаем _toRightButton, если boundaryValue = false
            if (toRightExclude == null || !toRightExclude.boundaryValue)
            {
                restoreSequence.Join(_toRightButton.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                    .Join(DOTween.To(() => _toRightButton.GetComponent<CanvasGroup>().alpha,
                                     x => _toRightButton.GetComponent<CanvasGroup>().alpha = x,
                                     1f, restoreDuration).SetEase(scaleEase));
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

            ExcludeFromHide toLeftExclude = _toLeftButton.GetComponent<ExcludeFromHide>();
            ExcludeFromHide toRightExclude = _toRightButton.GetComponent<ExcludeFromHide>();

            Sequence hideSequence = DOTween.Sequence();

            // Уменьшение масштаба и прозрачности для всех остальных объектов
            hideSequence.Append(aboutUsBotton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                .Join(DOTween.To(() => aboutUsBotton.GetComponent<CanvasGroup>().alpha, x => aboutUsBotton.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
                .Join(soundBotton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                .Join(DOTween.To(() => soundBotton.GetComponent<CanvasGroup>().alpha, x => soundBotton.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
                .Join(counterPanel.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                .Join(DOTween.To(() => counterPanel.GetComponent<CanvasGroup>().alpha, x => counterPanel.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
                .Join(chaptersNamesScrollable.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                .Join(DOTween.To(() => chaptersNamesScrollable.GetComponent<CanvasGroup>().alpha, x => chaptersNamesScrollable.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
                .Join(chaptersProgressScrollable.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                .Join(DOTween.To(() => chaptersProgressScrollable.GetComponent<CanvasGroup>().alpha, x => chaptersProgressScrollable.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
                .Join(circlesContainerPanel.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                .Join(DOTween.To(() => circlesContainerPanel.GetComponent<CanvasGroup>().alpha, x => circlesContainerPanel.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase));

            // Уменьшение масштаба и прозрачности _toLeftButton, если boundaryValue = false
            if (toLeftExclude == null || !toLeftExclude.boundaryValue)
            {
                hideSequence.Join(_toLeftButton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                    .Join(DOTween.To(() => _toLeftButton.GetComponent<CanvasGroup>().alpha, x => _toLeftButton.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase));
            }

            // Уменьшение масштаба и прозрачности _toRightButton, если boundaryValue = false
            if (toRightExclude == null || !toRightExclude.boundaryValue)
            {
                hideSequence.Join(_toRightButton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                    .Join(DOTween.To(() => _toRightButton.GetComponent<CanvasGroup>().alpha, x => _toRightButton.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase));
            }

            hideSequence.OnComplete(() =>
            {
                aboutUsBotton.gameObject.SetActive(false);
                chaptersNamesScrollable.gameObject.SetActive(false);
                chaptersProgressScrollable.gameObject.SetActive(false);
                circlesContainerPanel.gameObject.SetActive(false);

                if (toLeftExclude == null || !toLeftExclude.boundaryValue)
                    _toLeftButton.gameObject.SetActive(false);

                if (toRightExclude == null || !toRightExclude.boundaryValue)
                    _toRightButton.gameObject.SetActive(false);

                ShowLevelsList(true);
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