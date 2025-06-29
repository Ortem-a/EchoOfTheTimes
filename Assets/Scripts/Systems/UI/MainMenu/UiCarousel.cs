using System;
using UnityEngine;

namespace Systems.UI.MainMenu
{
    /// <summary>
    /// represents container for chapters.
    /// this is scrollable list with horizontal view.
    /// it's also contains pagination -> mini-carousel at bottom.
    /// Elements should slided smoothly.
    /// </summary>
    public class UiCarousel : MonoBehaviour
    {
        public Action OnMoveStarted;
        public Action OnMoveCompleted;

        private CarouselItem[] _items;

        public int CurrentSelected { get; private set; } = 0;

        private void Awake()
        {
            _items = GetComponentsInChildren<CarouselItem>();
        }

        public void MoveForward()
        {
            if (CurrentSelected + 1 == _items.Length) return;

            CurrentSelected++;
            _items[CurrentSelected].Select();
        }

        public void MoveBackward()
        {
            if (CurrentSelected == 0) return;

            CurrentSelected--;
            _items[CurrentSelected].Select();
        }
    }
}
