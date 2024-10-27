using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PanelPositioning : MonoBehaviour
{
    public RectTransform chapterPanel;
    private RectTransform levelPanel;

    public enum PositionSide
    {
        Right,
        Left,
        Top,
        Bottom
    }

    [Header("Сторона приклеивания")]
    public PositionSide positionSide = PositionSide.Right;

    void Start()
    {
        levelPanel = GetComponent<RectTransform>();

        // Определяем размеры chapterPanel для точного позиционирования
        float chapterWidth = chapterPanel.rect.width;
        float chapterHeight = chapterPanel.rect.height;

        switch (positionSide)
        {
            case PositionSide.Right:
                levelPanel.offsetMin = new Vector2(chapterWidth, levelPanel.offsetMin.y);
                levelPanel.offsetMax = new Vector2(chapterWidth, levelPanel.offsetMax.y);
                break;

            case PositionSide.Left:
                levelPanel.offsetMin = new Vector2(-chapterWidth, levelPanel.offsetMin.y);
                levelPanel.offsetMax = new Vector2(-chapterWidth, levelPanel.offsetMax.y);
                break;

            case PositionSide.Top:
                levelPanel.offsetMin = new Vector2(levelPanel.offsetMin.x, chapterHeight);
                levelPanel.offsetMax = new Vector2(levelPanel.offsetMax.x, chapterHeight);
                break;

            case PositionSide.Bottom:
                levelPanel.offsetMin = new Vector2(levelPanel.offsetMin.x, -chapterHeight);
                levelPanel.offsetMax = new Vector2(levelPanel.offsetMax.x, -chapterHeight);
                break;
        }
    }
}
