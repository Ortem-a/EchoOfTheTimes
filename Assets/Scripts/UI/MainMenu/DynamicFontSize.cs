using UnityEngine;
using TMPro;

public class DynamicFontSize : MonoBehaviour
{
    [SerializeField] private RectTransform targetCanvas;
    [SerializeField] private TMP_Text targetText;

    // Референсные значения
    private const float referenceWidth = 1393f;
    private const float referenceHeight = 2369f;
    private const float referenceFontSize = 54f;

    void Update()
    {
        if (targetCanvas != null && targetText != null)
        {
            float currentHeight = targetCanvas.rect.height;

            float scaleFactor = currentHeight / referenceHeight;

            targetText.fontSize = referenceFontSize * scaleFactor;
        }
    }
}
