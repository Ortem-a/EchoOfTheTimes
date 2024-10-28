using UnityEngine;
using UnityEngine.UI;

public class PanelPositionController : MonoBehaviour
{
    [Header("������ ��� ����������")]
    public RectTransform[] panels; // ��������� ���� ������ �����

    [Header("������ �� Scrollbar")]
    public Scrollbar scrollbar; // ������ �� ��� Scrollbar

    [Header("������ �� ������")]
    public RectTransform canvasRectTransform; // ������ �� RectTransform �������

    private int panelCount;
    private float shiftDistance;

    void Start()
    {
        panelCount = panels.Length;

        // �������� ������ ������� ��� shiftDistance
        shiftDistance = canvasRectTransform.rect.height;

        // �������������� ������� �������
        for (int i = 0; i < panelCount; i++)
        {
            if (i == 0)
            {
                // ������ ������ ������� ����
                panels[i].anchoredPosition = new Vector2(0, -shiftDistance);
            }
            else
            {
                // ��������� ������ �� ������� (0, 0)
                panels[i].anchoredPosition = Vector2.zero;
            }
        }
    }

    void Update()
    {
        // �������� �������� Scrollbar
        float value = scrollbar.value; // �������� �� 0 �� 1

        // ������������ ���������������� �������� � �������
        float scaledValue = value * (panelCount - 1);
        int index = Mathf.FloorToInt(scaledValue);
        index = Mathf.Clamp(index, 0, panelCount - 2);
        float t = scaledValue - index;

        // �������
        Vector2 inPosition = Vector2.zero;
        Vector2 outPosition = new Vector2(0, -shiftDistance);

        // ���������� ������ � ������������� ���������
        for (int i = 0; i < panelCount; i++)
        {
            if (i == index)
            {
                // ������� ������ ����������� �����
                panels[i].anchoredPosition = Vector2.Lerp(outPosition, inPosition, t);
            }
            else if (i == index + 1)
            {
                // ��������� ������ ���������� ����
                panels[i].anchoredPosition = Vector2.Lerp(inPosition, outPosition, t);
            }
            else
            {
                // ��������� ������ �������� �� ������� (0, 0)
                panels[i].anchoredPosition = inPosition;
            }
        }
    }
}
