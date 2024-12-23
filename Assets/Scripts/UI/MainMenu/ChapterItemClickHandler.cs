using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using DG.Tweening;
using EchoOfTheTimes.Persistence;

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

        [Header("Canvas ��� ����������")]
        [SerializeField] private RectTransform firstCanvas; // ������ Canvas ��� ����������
        [SerializeField] private RectTransform secondCanvas; // ������ Canvas ��� ����������
        [SerializeField] private float scaleDuration = 1f; // ����� ����� ���������� � ���������� Canvas
        [SerializeField] private float targetScale = 1.1f; // ������������ ����������
        [SerializeField] private Ease scaleEase = Ease.InOutQuad; // ����� Ease � ����������

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

        // ��� ��������� ���� ��� ����� � ������ ������ �� �������� ������, ��� ����� ���� �������, ��� �������� �����, ��� ������
        public void OnPointerClickSpecial(PointerEventData eventData)
        {
            _mainMenuService.SetActiveUi(false);

            _mainMenuService.HideElementsOfChapterMenu(this, true);

            Sequence canvasSequence = DOTween.Sequence();

            canvasSequence
              .Append(firstCanvas.DOScale(targetScale, scaleDuration / 2))
              .Join(secondCanvas.DOScale(targetScale, scaleDuration / 2))
              .Append(firstCanvas.DOScale(1f, scaleDuration / 2))
              .Join(secondCanvas.DOScale(1f, scaleDuration / 2));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_chapterStatus == StatusType.Locked) return;

            if (Mathf.Abs(progressHolder.change_progress_cf) < 0.999) return;

            _mainMenuService.SetActiveUi(false);

            Sequence canvasSequence = DOTween.Sequence();

            canvasSequence.OnStart(() => _mainMenuService.HideElementsOfChapterMenu(this))
                .Append(firstCanvas.DOScale(new Vector3(targetScale, targetScale * 1.1f, 1f), scaleDuration / 2))
                .Join(secondCanvas.DOScale(new Vector3(targetScale * 0.9f, targetScale * 0.9f, 1f), scaleDuration / 2))
                .Append(firstCanvas.DOScale(Vector3.one, scaleDuration / 2))
                .Join(secondCanvas.DOScale(Vector3.one, scaleDuration / 2));
        }
    }
}