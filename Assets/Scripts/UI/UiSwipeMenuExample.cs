using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSwipeMenuExample : MonoBehaviour
{
    [SerializeField] private Image _uiItemPrefab;
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private UiSwipeSnapChapter _swipeSnapMenu;
    [SerializeField] private TMP_Text _textSnappedIndex;
    [SerializeField] private TMP_Text _textSelectedIndex;

    private void OnEnable()
    {
        _swipeSnapMenu.TabSelected += OnTabSelected;
        _swipeSnapMenu.TabSnapped += OnTabSnapped;
    }

    private void OnDisable()
    {
        _swipeSnapMenu.TabSelected -= OnTabSelected;
        _swipeSnapMenu.TabSnapped -= OnTabSnapped;
    }

    public void AddItem()
    {
        var img = Instantiate(_uiItemPrefab, _itemsContainer);
        img.color = GetRandomColor();

        _swipeSnapMenu.Recalculate();
    }

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

    private Color GetRandomColor()
    {
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);

        return new Color(r, g, b);
    }

    private void OnTabSelected(int tabIndex)
    {
        _textSelectedIndex.text = tabIndex.ToString();
    }

    private void OnTabSnapped(int tabIndex)
    {
        _textSnappedIndex.text = tabIndex.ToString();
    }
}
