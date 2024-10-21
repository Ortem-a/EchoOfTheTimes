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
        [SerializeField] private RectTransform _referenceRect; // ������ ��� ������ ����� = ������ ������ �����
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
        public float change_progress_cf = 0;

        private bool _isDragging;
        private bool _isSnapping;
        private readonly List<float> _itemPositions = new List<float>(); // ������� � ��������, ����� ���� ����� ����� � ���� �� ������� ���������, ���� ����, ������ ����������)
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

            // ShowChangeChapterProgress(); // ���������, ����� ������� ����� � �������� �������� �� ��� ���, ���� �� ����������
        }

        // ������ �� ���������� ������, ����� ������������ ��� �������� ��������� ������� ��������_���������, ����� ����, ��������� � ��������� �������
        public void OnDrag(PointerEventData eventData)
        {
            ShowChangeChapterProgress();
        }

        private void ShowChangeChapterProgressInvert()
        {
            float scrollPos = _scrollRect.horizontalNormalizedPosition * (_contentContainer.rect.width - _referenceRect.rect.width);
            float currentItemPos = _itemPositions[SelectedTabIndex];

            float distance = scrollPos - currentItemPos;

            if (distance >= 0)
            {
                change_progress_cf = -1f * (1f - distance / _itemWidth);
            }
            else
            {
                change_progress_cf = 1f + distance / _itemWidth;
            }
        }

        private void ShowChangeChapterProgress()
        {
            float scrollPos = _scrollRect.horizontalNormalizedPosition * (_contentContainer.rect.width - _referenceRect.rect.width);
            float currentItemPos = _itemPositions[SelectedTabIndex];

            float distance = scrollPos - currentItemPos;

            change_progress_cf = distance / _itemWidth;
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

            // ���������� ������ ������ ����������� ������� (_referenceRect)
            _itemWidth = _referenceRect.rect.width;

            // ������������ ������� ��������� � ��������, �� ������ �� ������� � ������
            for (var i = 0; i < itemsCount; i++)
            {
                float itemPosition = i * _itemWidth;
                _itemPositions.Add(itemPosition);
            }

            // ��������, ��� ������� �� ��������� ������� �����
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
            float scrollPos = _scrollRect.horizontalNormalizedPosition * (_contentContainer.rect.width - _referenceRect.rect.width);
            float currentItemPos = _itemPositions[SelectedTabIndex];

            float distance = scrollPos - currentItemPos;

            // ����� ��� ����� ��������: ������� �� ������ �������� (��������, 25%)
            float swipeThreshold = _itemWidth * 0.25f; // ������������� ��������

            // change_progress_cf = distance / _itemWidth; // ��� ��������� ������ � ������ ����� ������, � ���� � ��������� ���-��

            if (Mathf.Abs(distance) > swipeThreshold)
            {
                if (distance > 0 && SelectedTabIndex < _itemPositions.Count - 1)
                {
                    // ����� ������: ��������� �� ��������� �������
                    SelectedTabIndex++;
                }
                else if (distance < 0 && SelectedTabIndex > 0)
                {
                    // ����� �����: ������������ �� ���������� �������
                    SelectedTabIndex--;
                }
            }

            SelectTab(SelectedTabIndex);
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

            // change_progress_cf = (_scrollRect.horizontalNormalizedPosition - targetPosition) / _itemWidth;

            // change_progress_cf = _scrollRect.horizontalNormalizedPosition;

            ShowChangeChapterProgressInvert();

            if (Mathf.Abs(_scrollRect.horizontalNormalizedPosition - targetPosition) <= 0.0001f)
            {
                _scrollRect.horizontalNormalizedPosition = targetPosition;
                _isSnapping = false;
                TabSnapped?.Invoke(SelectedTabIndex);
            }
        }
    }
}
