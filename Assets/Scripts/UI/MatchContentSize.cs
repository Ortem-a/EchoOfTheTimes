using UnityEngine;

public class MatchCanvasWidth : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;
    private RectTransform _chapterButtonRect;

    private void Start()
    {
        _chapterButtonRect = GetComponent<RectTransform>();

        if (_chapterButtonRect != null && canvasRect != null)
        {
            MatchWidth();
        }
    }

    private void MatchWidth()
    {
        _chapterButtonRect.sizeDelta = new Vector2(canvasRect.rect.width, _chapterButtonRect.sizeDelta.y);
    }
}