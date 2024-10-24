using UnityEngine;

public class AdjustPanelWithAspectRatio : MonoBehaviour
{
    [SerializeField] private RectTransform firstPanel;  
    [SerializeField] private RectTransform secondPanel;
    [SerializeField] private RectTransform thirdPanel;
    [SerializeField] private RectTransform fourthPanel;
    [SerializeField] private bool isChapter = true;
    [SerializeField] private int countLevels = 6;

    [SerializeField] private float aspectRatio = 716f / 1281f; // ����������� ������ � ������

    private void Start()
    {
        if (isChapter)
        {
            AdjustPanelSize();
        }
        else
        {
            AdjustLevelPanelSize();
        }
        
    }

    private void AdjustPanelSize()
    {
        // �������� �������� ������ � ������
        float originalWidth = firstPanel.rect.width;
        float originalHeight = firstPanel.rect.height;

        // �������������
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

    private void AdjustLevelPanelSize()
    {
        // �������� �������� ������ � ������
        float originalWidth = firstPanel.rect.width;
        float originalHeight = firstPanel.rect.height;

        // �������������
        float correctedHeight = (1 - (thirdPanel.anchorMin[1] + (1 - thirdPanel.anchorMax[1]))) *
                                originalHeight *
                                (1 - (secondPanel.anchorMin[1] + (1 - secondPanel.anchorMax[1]))) *
                                (1 - (fourthPanel.anchorMin[1] + (1 - fourthPanel.anchorMax[1]))) / countLevels;

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
