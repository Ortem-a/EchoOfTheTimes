using UnityEngine;

public class HideOnBoundaryChapter : MonoBehaviour
{
    public UiSwipeSnapChapter swipeSnapChapter;
    public bool hideOnFirstChapter = false;
    public bool hideOnLastChapter = false;

    private void Start()
    {
        if (swipeSnapChapter != null)
        {
            swipeSnapChapter.TabSelected += OnTabSelected;
            UpdateVisibility(swipeSnapChapter.SelectedTabIndex);
        }
    }

    private void OnDestroy()
    {
        if (swipeSnapChapter != null)
        {
            swipeSnapChapter.TabSelected -= OnTabSelected;
        }
    }

    private void OnTabSelected(int index)
    {
        UpdateVisibility(index);
    }

    private void UpdateVisibility(int index)
    {
        int lastIndex = swipeSnapChapter.ItemCount - 1;

        if ((hideOnFirstChapter && index == 0) || (hideOnLastChapter && index == lastIndex))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
