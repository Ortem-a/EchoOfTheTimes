using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using DG.Tweening;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterItemClickHandler : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField]
        public LevelStatusUpdater LevelsPanel { get; private set; }

        private UiMainMenuService _mainMenuService;

        [SerializeField]
        private UiSwipeSnapChapter progressHolder;

        private ChapterLockView _chapterLockView;

        [Header("Canvas для увеличения")]
        [SerializeField] private RectTransform firstCanvas; // Первый Canvas для увеличения
        [SerializeField] private RectTransform secondCanvas; // Второй Canvas для увеличения
        [SerializeField] private float scaleDuration = 1f; // Общее время увеличения и уменьшения Canvas
        [SerializeField] private float targetScale = 1.1f; // Максимальное увеличение
        [SerializeField] private Ease scaleEase = Ease.InOutQuad; // Выбор Ease в инспекторе

        private StatusType _chapterStatus;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _mainMenuService = mainMenuService;

            _chapterLockView = GetComponentInChildren<ChapterLockView>();
        }

        public void SetStatus(StatusType status)
        {
            _chapterStatus = status;

            if (status != StatusType.Locked)
            {
                _chapterLockView.Unlock();
            }
        }

        public void SetProgress(int progress, int required)
        {
            _chapterLockView.UpdateLabel(progress, required);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_chapterStatus == StatusType.Locked) return;

            if (Mathf.Abs(progressHolder.change_progress_cf) < 0.999) return;

#warning ДОБАВИТЬ ОТКЛЮЧЕНИЕ ИНТЕРАКТИВНОСТИ ИНТЕРФЕЙСА В МОМЕНТ ТАПА В ГЛАВУ
            _mainMenuService.SetActiveUi(false);

            Sequence canvasSequence = DOTween.Sequence();

            canvasSequence.OnStart(() => _mainMenuService.HideElementsOfChapterMenu(this))
              .Append(firstCanvas.DOScale(targetScale, scaleDuration / 2))
              .Join(secondCanvas.DOScale(targetScale, scaleDuration / 2))
              .Append(firstCanvas.DOScale(1f, scaleDuration / 2))
              .Join(secondCanvas.DOScale(1f, scaleDuration / 2));
        }
    }
}