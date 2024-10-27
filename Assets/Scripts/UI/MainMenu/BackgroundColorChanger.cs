using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    [Header("������ ������ ��� ������ �����")]
    public Color[] chapterColors = new Color[8];

    [Header("������ �� Scrollbar")]
    public Scrollbar scrollbar;

    private Image panelImage;

    void Start()
    {
        panelImage = GetComponent<Image>();
    }

    void Update()
    {
        float value = scrollbar.value; // �������� �� 0 �� 1

        int chaptersCount = chapterColors.Length;

        // ������������ ������ �������� ���������
        float scaledValue = value * (chaptersCount - 1);
        int index = Mathf.FloorToInt(scaledValue);

        // ������������ ������ � �������� �������
        index = Mathf.Clamp(index, 0, chaptersCount - 2);

        // ������������ ����������� ������������ t
        float t = scaledValue - index;

        // �������� ����� ��� ������������
        Color fromColor = chapterColors[index];
        Color toColor = chapterColors[index + 1];

        // ��������� ������������ �����
        Color interpolatedColor = Color.Lerp(fromColor, toColor, t);

        // ������������� ���� ������
        panelImage.color = interpolatedColor;
    }
}
