using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PanelPositioning : MonoBehaviour
{
    public RectTransform chapterPanel;
    private RectTransform levelPanel;

    void Start()
    {
        levelPanel = GetComponent<RectTransform>();

        RectTransform parentCanvas = chapterPanel.parent.GetComponent<RectTransform>();
        float canvasWidth = parentCanvas.rect.width;

        levelPanel.offsetMin = new Vector2(canvasWidth, levelPanel.offsetMin.y);
        levelPanel.offsetMax = new Vector2(canvasWidth, levelPanel.offsetMax.y);
    }
}
