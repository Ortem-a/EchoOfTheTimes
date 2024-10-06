using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSwipeSnapChapter : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public event Action<int> TabSelected;
    public event Action<int > TabSnapped;

    [SerializeField] private RectTransform _contentContainer;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private float _snapSpeed = 15;

    public int SelectedTabIndex => _selectedTabIndex;

    private bool _isDragging;
    private bool _isSnapping;
    private readonly List<float> _itemPositionsNormalized = new List<float>(); // 0-1 значения шкалы
    private float _targetScrollBarValueNormalized = 0;
    private float _itemSizeNormalized;
    private int _selectedTabIndex;

    private void Start()
    {
        Recalculate();
    }

    private void Update()
    {
        if (_isDragging)
        {  
            return; 
        }

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

    public void OnEndDrag(PointerEventData eventData)
    {
        _targetScrollBarValueNormalized = _scrollbar.value;
        _isDragging = false;
        _isSnapping = true;

        FindSnappingTabAndStartSnapping();
    }

    // Пересчитываем лейаут в том же кадре
    public void Recalculate()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);

        _itemPositionsNormalized.Clear();

        var itemsCount = _contentContainer.childCount;
        _itemSizeNormalized = 1f / (itemsCount - 1f);

        for(var i = 0; i < itemsCount; i++)
        {
            var itemPositionNormalized = _itemSizeNormalized * i;

            _itemPositionsNormalized.Add(itemPositionNormalized);
        }

        SelectTab(_selectedTabIndex + 1);
    }

    public void SelectTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
        {
            return;
        }

        _selectedTabIndex = tabIndex;
        _targetScrollBarValueNormalized = _itemPositionsNormalized[tabIndex];
        _isSnapping = true;

        TabSelected?.Invoke(tabIndex);
    }

    private void FindSnappingTabAndStartSnapping()
    {
        for (int i = 0; i < _itemPositionsNormalized.Count; i++)
        {
            var itemPositionNormalized = _itemPositionsNormalized[i];

            if (_targetScrollBarValueNormalized < itemPositionNormalized + _itemSizeNormalized / 2f
                && _targetScrollBarValueNormalized > itemPositionNormalized - _itemSizeNormalized / 2f)
            {
                SelectTab(i);
                break;
            }
        }
    }

    private void SnapContent()
    {
        if (_itemPositionsNormalized.Count < 2)
        {
            _isSnapping = false;
            return;
        }

        var targetPosition = _itemPositionsNormalized[_selectedTabIndex];
        _scrollbar.value = Mathf.Lerp(_scrollbar.value, targetPosition, Time.deltaTime * _snapSpeed);

        if (Mathf.Abs(_scrollbar.value - targetPosition) <= 0.0001f)
        {
            _isSnapping = false;
            TabSnapped?.Invoke(_selectedTabIndex);
        }
    }
}