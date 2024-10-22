using UnityEngine;

public class AdjustPanelWithAspectRatio : MonoBehaviour
{
    [SerializeField] private RectTransform firstPanel;  
    [SerializeField] private RectTransform secondPanel;
    [SerializeField] private RectTransform thirdPanel;

    [SerializeField] private float aspectRatio = 716f / 1281f; // ����������� ������ � ������

    private void Start()
    {
        if (firstPanel != null && secondPanel != null && thirdPanel != null) 
        {
            AdjustPanelSize();
        }
        else if (firstPanel == null && secondPanel == null && thirdPanel == null)
        {
            AdjustPanelSizeJust();
        }
    }

    private void AdjustPanelSizeJust()
    {
        return;
    }


    private void AdjustPanelSize()
    {
        // �������� �������� ������ � ������
        float originalWidth = firstPanel.rect.width;
        float originalHeight = firstPanel.rect.height;

        // �������������
        // float correctedWidth = originalWidth * secondPanel.anchorMin[1];
        float correctedHeight = (1 - (thirdPanel.anchorMin[1] + (1 - thirdPanel.anchorMax[1]))) *
                                originalHeight *
                                (1 - (secondPanel.anchorMin[1] + (1 - secondPanel.anchorMax[1])));

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
