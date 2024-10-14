using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class UiSwipeMenu : MonoBehaviour
    {
        [SerializeField]
        private UiSwipeSnapChapter _swipeSnapMenu;

        public void SlideNext()
        {
            _swipeSnapMenu.SlideNext();

            //var index = _swipeSnapMenu.SelectedTabIndex;
            //_swipeSnapMenu.SelectTab(index + 1);
        }

        public void SlidePrevious()
        {
            _swipeSnapMenu.SlidePrevious();

            //var index = _swipeSnapMenu.SelectedTabIndex;
            //_swipeSnapMenu.SelectTab(index - 1);
        }
    }
}