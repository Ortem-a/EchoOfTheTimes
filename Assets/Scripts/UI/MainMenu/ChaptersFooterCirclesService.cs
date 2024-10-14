using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChaptersFooterCirclesService : MonoBehaviour
    {
        private FooterCircleEffectsService[] _circles;

        private float _defaultWidth = 25f;
        private float _defaultHeight = 25f;

        private float _multiplyer = 2f;

        private int _lastIndex = -1;

        private void Awake()
        {
            _circles = GetComponentsInChildren<FooterCircleEffectsService>();

            UiSwipeSnapChapter.OnChapterSwiped += HandleSwipe;
        }

        private void OnDestroy()
        {
            UiSwipeSnapChapter.OnChapterSwiped -= HandleSwipe;
        }

        private void HandleSwipe(int activeCirleIndex)
        {
            if (_lastIndex != -1)
            {
                _circles[_lastIndex].MarkAsSelected(false);
            }

            _lastIndex = activeCirleIndex;

            _circles[_lastIndex].MarkAsSelected(true, _multiplyer);
        }
    }
}