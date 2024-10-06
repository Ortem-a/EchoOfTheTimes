using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSwipeSnapChapter : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<int> TabSelected;
    public event Action<int> TabSnapped;

    [SerializeField] private RectTransform _contentContainer;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private float _snapSpeed = 15;

    public int SelectedTabIndex => _selectedTabIndex;
    public int ItemCount => _itemPositionsNormalized.Count;

    private bool _isDragging;
    private bool _isSnapping;
    private readonly List<float> _itemPositionsNormalized = new List<float>();
    private float _targetScrollPosition = 0;
    private float _itemSizeNormalized;
    private int _selectedTabIndex;

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

        SelectTab(_selectedTabIndex);
    }

    public void SelectTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
        {
            return;
        }

        _selectedTabIndex = tabIndex;
        _targetScrollPosition = _itemPositionsNormalized[tabIndex];
        _isSnapping = true;

        TabSelected?.Invoke(tabIndex);
    }

    public void SlideNext()
    {
        SelectTab(_selectedTabIndex + 1);
    }

    public void SlidePrevious()
    {
        SelectTab(_selectedTabIndex - 1);
    }

    private void FindSnappingTabAndStartSnapping()
    {
        float closestPosition = float.MaxValue;
        int closestIndex = _selectedTabIndex;

        for (int i = 0; i < _itemPositionsNormalized.Count; i++)
        {
            float distance = Mathf.Abs(_scrollRect.horizontalNormalizedPosition - _itemPositionsNormalized[i]);
            if (distance < closestPosition)
            {
                closestPosition = distance;
                closestIndex = i;
            }
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

        var targetPosition = _itemPositionsNormalized[_selectedTabIndex];
        _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(_scrollRect.horizontalNormalizedPosition, targetPosition, Time.deltaTime * _snapSpeed);

        if (Mathf.Abs(_scrollRect.horizontalNormalizedPosition - targetPosition) <= 0.0001f)
        {
            _scrollRect.horizontalNormalizedPosition = targetPosition;
            _isSnapping = false;
            TabSnapped?.Invoke(_selectedTabIndex);
        }
    }
}
