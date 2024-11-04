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
        [SerializeField] private RectTransform soundButton;
        [SerializeField] private RectTransform aboutUsButton;
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

        [Header("Для стартового растемнения")]
        [SerializeField] CanvasGroup _fadeCanvasGroup;
        [SerializeField] private float fadeDuration = 0.5f;

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

            _fadeCanvasGroup.alpha = 1f;

            FadeIn();
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

        public void FadeIn()
        {
            Debug.Log("FadeIn started");

            _fadeCanvasGroup.gameObject.SetActive(true);

            SetActiveUi(false);

            DOTween.To(() => _fadeCanvasGroup.alpha, x => _fadeCanvasGroup.alpha = x, 0f, fadeDuration)
                .OnComplete(() =>
                {
                    _fadeCanvasGroup.gameObject.SetActive(false);

                    SetActiveUi(true);
                });
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

            float durationTrans = durationTransitionBeetweenPanels;
            float delayToLevels = 0.3f;
            float delayToChapters = 0f;

            if (isLevels)
            {
                // Анимация для первой панели (уходит влево за экран)
                DOTween.To(() => chaptersPanel.offsetMin, x => chaptersPanel.offsetMin = x, new Vector2(-canvasWidth, chaptersPanel.offsetMin.y), durationTrans)
                    .SetDelay(delayToLevels)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => chaptersPanel.offsetMax, x => chaptersPanel.offsetMax = x, new Vector2(-canvasWidth, chaptersPanel.offsetMax.y), durationTrans)
                    .SetDelay(delayToLevels)
                    .SetEase(Ease.InOutQuad);

                // Анимация для второй панели (вплывает справа на место первой панели)
                DOTween.To(() => levelsPanel.offsetMin, x => levelsPanel.offsetMin = x, Vector2.zero, durationTrans)
                    .SetDelay(delayToLevels)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => levelsPanel.offsetMax, x => levelsPanel.offsetMax = x, Vector2.zero, durationTrans)
                    .SetDelay(delayToLevels)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => SetActiveUi(true)); // Включаем взаимодействие после завершения анимации
            }
            else
            {
                // Анимация для первой панели (возвращается на место)
                DOTween.To(() => chaptersPanel.offsetMin, x => chaptersPanel.offsetMin = x, Vector2.zero, durationTrans)
                    .SetDelay(delayToChapters)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => chaptersPanel.offsetMax, x => chaptersPanel.offsetMax = x, Vector2.zero, durationTrans)
                    .SetDelay(delayToChapters)
                    .SetEase(Ease.InOutQuad);

                // Анимация для второй панели (уходит вправо за экран)
                DOTween.To(() => levelsPanel.offsetMin, x => levelsPanel.offsetMin = x, new Vector2(canvasWidth, levelsPanel.offsetMin.y), durationTrans)
                    .SetDelay(delayToChapters)
                    .SetEase(Ease.InOutQuad);

                DOTween.To(() => levelsPanel.offsetMax, x => levelsPanel.offsetMax = x, new Vector2(canvasWidth, levelsPanel.offsetMax.y), durationTrans)
                    .SetDelay(delayToChapters)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        SetActiveUi(true); // включается раньше окончания растемнения?

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
            aboutUsButton.gameObject.SetActive(true);
            soundButton.gameObject.SetActive(true);
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

            restoreSequence.Append(aboutUsButton.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => aboutUsButton.GetComponent<CanvasGroup>().alpha,
                                 x => aboutUsButton.GetComponent<CanvasGroup>().alpha = x,
                                 1f, restoreDuration).SetEase(scaleEase))
                .Join(soundButton.transform.DOScale(1f, restoreDuration).SetEase(scaleEase))
                .Join(DOTween.To(() => soundButton.GetComponent<CanvasGroup>().alpha,
                                 x => soundButton.GetComponent<CanvasGroup>().alpha = x,
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


        public void HideElementsOfChapterMenu(ChapterItemClickHandler chapterUiItem, bool isMomentalSwipe = false)
        {
            if (isMomentalSwipe)
            {
                _lastChapterUiItem = chapterUiItem;

                ExcludeFromHide toLeftExclude = _toLeftButton.GetComponent<ExcludeFromHide>();
                ExcludeFromHide toRightExclude = _toRightButton.GetComponent<ExcludeFromHide>();

                aboutUsButton.gameObject.SetActive(false);
                chaptersNamesScrollable.gameObject.SetActive(false);
                chaptersProgressScrollable.gameObject.SetActive(false);
                circlesContainerPanel.gameObject.SetActive(false);
                soundButton.gameObject.SetActive(false);
                counterPanel.gameObject.SetActive(false);

                if (toLeftExclude == null || !toLeftExclude.boundaryValue)
                    _toLeftButton.gameObject.SetActive(false);

                if (toRightExclude == null || !toRightExclude.boundaryValue)
                    _toRightButton.gameObject.SetActive(false);
                
                ShowLevelsList(true);
            }
            else
            {
                _lastChapterUiItem = chapterUiItem;

                EnsureCanvasGroup(soundButton.gameObject);
                EnsureCanvasGroup(aboutUsButton.gameObject);
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
                hideSequence.Append(aboutUsButton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                    .Join(DOTween.To(() => aboutUsButton.GetComponent<CanvasGroup>().alpha, x => aboutUsButton.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
                    .Join(soundButton.transform.DOScale(targetScale, scaleDurationN).SetEase(scaleEase))
                    .Join(DOTween.To(() => soundButton.GetComponent<CanvasGroup>().alpha, x => soundButton.GetComponent<CanvasGroup>().alpha = x, 0, scaleDurationN).SetEase(scaleEase))
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
                    aboutUsButton.gameObject.SetActive(false);
                    chaptersNamesScrollable.gameObject.SetActive(false);
                    chaptersProgressScrollable.gameObject.SetActive(false);
                    circlesContainerPanel.gameObject.SetActive(false);
                    soundButton.gameObject.SetActive(false);
                    counterPanel.gameObject.SetActive(false);

                    if (toLeftExclude == null || !toLeftExclude.boundaryValue)
                        _toLeftButton.gameObject.SetActive(false);

                    if (toRightExclude == null || !toRightExclude.boundaryValue)
                        _toRightButton.gameObject.SetActive(false);

                    ShowLevelsList(true);
                });
            }  
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