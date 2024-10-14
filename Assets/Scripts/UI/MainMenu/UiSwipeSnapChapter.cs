using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class UiSwipeSnapChapter : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Action<int> TabSelected;
        public Action<int> TabSnapped;

        public static Action<int> OnChapterSwiped;

        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _snapSpeed = 15;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            private set
            {
                _selectedTabIndex = value;
                OnChapterSwiped?.Invoke(value);
            }
        }
        public int ItemCount => _itemPositionsNormalized.Count;

        private bool _isDragging;
        private bool _isSnapping;
        private readonly List<float> _itemPositionsNormalized = new List<float>();
        private float _targetScrollPosition = 0;
        private float _itemSizeNormalized;

        private void Start()
        {
            Recalculate();
        }

        private void Update()
        {
            if (_isSnapping)
            {
                SnapContent();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _isSnapping = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // События свайпа обрабатываются ScrollRect
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            _isSnapping = true;

            FindSnappingTabAndStartSnapping();
        }

        public void Recalculate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);

            _itemPositionsNormalized.Clear();

            var itemsCount = _contentContainer.childCount;
            _itemSizeNormalized = 1f / (itemsCount - 1f);

            for (var i = 0; i < itemsCount; i++)
            {
                var itemPositionNormalized = _itemSizeNormalized * i;
                _itemPositionsNormalized.Add(itemPositionNormalized);
            }

            SelectTab(SelectedTabIndex);
        }

        public void SelectTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
            {
                return;
            }

            SelectedTabIndex = tabIndex;
            _targetScrollPosition = _itemPositionsNormalized[tabIndex];
            _isSnapping = true;

            TabSelected?.Invoke(tabIndex);
        }

        public void SlideNext()
        {
            SelectTab(SelectedTabIndex + 1);
        }

        public void SlidePrevious()
        {
            SelectTab(SelectedTabIndex - 1);
        }

        private void FindSnappingTabAndStartSnapping()
        {
            float closestPosition = float.MaxValue;
            int closestIndex = SelectedTabIndex;

            // Порог для смены страницы: 1/3 от размера элемента
            float swipeThreshold = _itemSizeNormalized / 3f;

            for (int i = 0; i < _itemPositionsNormalized.Count; i++)
            {
                float distance = Mathf.Abs(_scrollRect.horizontalNormalizedPosition - _itemPositionsNormalized[i]);
                if (distance < closestPosition)
                {
                    closestPosition = distance;
                    closestIndex = i;
                }
            }

            // Переход на предыдущий или следующий элемент при достижении порога свайпа
            if (_scrollRect.horizontalNormalizedPosition > _itemPositionsNormalized[SelectedTabIndex] + swipeThreshold && SelectedTabIndex < _itemPositionsNormalized.Count - 1)
            {
                closestIndex = SelectedTabIndex + 1; // Свайп вправо
            }
            else if (_scrollRect.horizontalNormalizedPosition < _itemPositionsNormalized[SelectedTabIndex] - swipeThreshold && SelectedTabIndex > 0)
            {
                closestIndex = SelectedTabIndex - 1; // Свайп влево
            }

            SelectTab(closestIndex);
        }

        private void SnapContent()
        {
            if (_itemPositionsNormalized.Count < 2)
            {
                _isSnapping = false;
                return;
            }

            var targetPosition = _itemPositionsNormalized[SelectedTabIndex];
            _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(_scrollRect.horizontalNormalizedPosition, targetPosition, Time.deltaTime * _snapSpeed);

            if (Mathf.Abs(_scrollRect.horizontalNormalizedPosition - targetPosition) <= 0.0001f)
            {
                _scrollRect.horizontalNormalizedPosition = targetPosition;
                _isSnapping = false;
                TabSnapped?.Invoke(SelectedTabIndex);
            }
        }
    }
}