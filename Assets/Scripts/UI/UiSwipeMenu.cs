using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSwipeMenu : MonoBehaviour
{
    [SerializeField] private Image _uiItemPrefab;
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private UiSwipeSnapChapter _swipeSnapMenu;

    public void SlideNext()
    {
        var index = _swipeSnapMenu.SelectedTabIndex;
        _swipeSnapMenu.SelectTab(index + 1);
    }

    public void SlidePrevious()
    {
        var index = _swipeSnapMenu.SelectedTabIndex;
        _swipeSnapMenu.SelectTab(index - 1);
    }
}
