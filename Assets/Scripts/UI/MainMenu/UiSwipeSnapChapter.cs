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
        [SerializeField] private RectTransform _referenceRect; // Объект, чью ширину будем использовать
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
        public int ItemCount = 8;

        private bool _isDragging;
        private bool _isSnapping;
        private readonly List<float> _itemPositions = new List<float>(); // Позиции в пикселях
        private float _targetScrollPosition = 0;
        private float _itemWidth;

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

            _itemPositions.Clear();

            var itemsCount = _contentContainer.childCount;

            // Используем ТОЛЬКО ширину переданного объекта (_referenceRect)
            _itemWidth = _referenceRect.rect.width;

            // Рассчитываем позиции элементов в пикселях, на основе их индекса и ширины
            for (var i = 0; i < itemsCount; i++)
            {
                float itemPosition = i * _itemWidth;
                _itemPositions.Add(itemPosition);
            }

            // Убедимся, что позиция на выбранной вкладке верна
            SelectTab(SelectedTabIndex);
        }

        public void SelectTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _itemPositions.Count)
            {
                return;
            }

            SelectedTabIndex = tabIndex;
            _targetScrollPosition = _itemPositions[tabIndex] / (_contentContainer.rect.width - _referenceRect.rect.width);
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

            // Порог для смены страницы: 1/3 от ширины объекта (_referenceRect)
            float swipeThreshold = _itemWidth / 3f;

            for (int i = 0; i < _itemPositions.Count; i++)
            {
                float distance = Mathf.Abs(_scrollRect.horizontalNormalizedPosition * (_contentContainer.rect.width - _referenceRect.rect.width) - _itemPositions[i]);
                if (distance < closestPosition)
                {
                    closestPosition = distance;
                    closestIndex = i;
                }
            }

            // Переход на предыдущий или следующий элемент при достижении порога свайпа
            if (_scrollRect.horizontalNormalizedPosition > _itemPositions[SelectedTabIndex] / (_contentContainer.rect.width - _referenceRect.rect.width) + swipeThreshold && SelectedTabIndex < _itemPositions.Count - 1)
            {
                closestIndex = SelectedTabIndex + 1; // Свайп вправо
            }
            else if (_scrollRect.horizontalNormalizedPosition < _itemPositions[SelectedTabIndex] / (_contentContainer.rect.width - _referenceRect.rect.width) - swipeThreshold && SelectedTabIndex > 0)
            {
                closestIndex = SelectedTabIndex - 1; // Свайп влево
            }

            SelectTab(closestIndex);
        }

        private void SnapContent()
        {
            if (_itemPositions.Count < 2)
            {
                _isSnapping = false;
                return;
            }

            var targetPosition = _itemPositions[SelectedTabIndex] / (_contentContainer.rect.width - _referenceRect.rect.width);
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
