using UnityEngine;
using System.Collections;

public class ScreenTransitionManager : MonoBehaviour
{
    public RectTransform firstPanel;
    public RectTransform secondPanel;
    public float transitionDuration = 1f; // �����, �� ������� ������ ������������
    public float delayBeforeTransition = 2f; // �������� ����� ������� ��������

    private Vector2 screenSize;

    private void Start()
    {
        // �������� ������ ������ � �������� Canvas
        Canvas canvas = GetComponent<Canvas>();
        screenSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // ������������� ��������� ������� �������
        SetInitialPositions();

        // ��������� �������� ��� ��������
        StartCoroutine(StartTransition());
    }

    private void SetInitialPositions()
    {
        // ������������� FirstPanel � �����
        firstPanel.anchoredPosition = Vector2.zero;

        // ������������� SecondPanel �� ������ ���� ������
        secondPanel.anchoredPosition = new Vector2(screenSize.x, 0);
    }

    private IEnumerator StartTransition()
    {
        // ���� �������� ��������
        yield return new WaitForSeconds(delayBeforeTransition);

        // ��������� ������������ ����������� �������
        StartCoroutine(MovePanel(firstPanel, new Vector2(-screenSize.x, 0), transitionDuration));
        StartCoroutine(MovePanel(secondPanel, Vector2.zero, transitionDuration));
    }

    private IEnumerator MovePanel(RectTransform panel, Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // ������������� ������ �������� �������
        panel.anchoredPosition = targetPosition;
    }
}
