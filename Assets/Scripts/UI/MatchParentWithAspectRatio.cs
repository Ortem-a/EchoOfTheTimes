using UnityEngine;

public class AdjustPanelWithAspectRatio : MonoBehaviour
{
    [SerializeField] private RectTransform firstPanel;  
    [SerializeField] private RectTransform secondPanel;

    private const float aspectRatio = 716f / 1281f; // ����������� ������ � ������

    private void Start()
    {
        AdjustPanelSize();
    }

    private void AdjustPanelSize()
    {
        if (firstPanel == null)
        {
            Debug.LogError("RectTransform �� ��������!");
            return;
        }

        // �������� �������� ������ � ������
        float originalWidth = firstPanel.rect.width;
        float originalHeight = firstPanel.rect.height;

        // �������������
        // float correctedWidth = originalWidth * secondPanel.anchorMin[1];
        float correctedHeight = 0.7f * originalHeight * (1 - (secondPanel.anchorMin[1] + (1 - secondPanel.anchorMax[1])));

        // ��������� ����� ������ �� ������ ��������� �����������
        float newWidth = correctedHeight * aspectRatio;

        // ��������� ������� � ����� � �� 2
        float difference = (originalWidth - newWidth) / 2f;

        // ����������� �������� � Left � Right
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        thisRectTransform.offsetMin = new Vector2(difference, thisRectTransform.offsetMin.y); // Left
        thisRectTransform.offsetMax = new Vector2(-difference, thisRectTransform.offsetMax.y); // Right
    }
}
