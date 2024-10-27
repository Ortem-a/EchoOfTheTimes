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

        if (panelCount < 2)
        {
            Debug.LogError("���������� ������� ��� ������� 2 ������.");
            return;
        }

        if (canvasRectTransform == null)
        {
            Debug.LogError("���������� ��������� RectTransform �������.");
            return;
        }

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
        // ��������� ������� Scrollbar
        if (scrollbar == null)
        {
            Debug.LogError("���������� ��������� Scrollbar.");
            return;
        }

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

        // ���������� ������ � ���������� ���������
        for (int i = 0; i < panelCount; i++)
        {
            if (i == index)
            {
                // ������� ������
                if (t <= 0.5f)
                {
                    // ������ ����: ������ ����������� �����
                    float phaseT = t / 0.5f; // ����������� t �� 0 �� 0.5 � 0 �� 1
                    panels[i].anchoredPosition = Vector2.Lerp(outPosition, inPosition, phaseT);
                }
                else
                {
                    // ������ ����: ������ �������� �� �����
                    panels[i].anchoredPosition = inPosition;
                }
            }
            else if (i == index + 1)
            {
                // ��������� ������
                if (t > 0.5f)
                {
                    // ������ ����: ������ ���������� ����
                    float phaseT = (t - 0.5f) / 0.5f; // ����������� t �� 0.5 �� 1 � 0 �� 1
                    panels[i].anchoredPosition = Vector2.Lerp(inPosition, outPosition, phaseT);
                }
                else
                {
                    // ������ ����: ������ �������� �� �����
                    panels[i].anchoredPosition = inPosition;
                }
            }
            else
            {
                // ��������� ������ �������� �� ������� (0, 0)
                panels[i].anchoredPosition = inPosition;
            }
        }
    }
}
