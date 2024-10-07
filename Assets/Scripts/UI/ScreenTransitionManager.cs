using UnityEngine;
using System.Collections;
using System;

public class ScreenTransitionManager : MonoBehaviour
{
    public RectTransform firstPanel;
    public RectTransform secondPanel;
    public float transitionDuration = 1f; // ����� ����������� �������
    public float delayBeforeTransition = 2f; // �������� ����� ������� ��������

    private Vector2 screenSize;
    private bool isTransitioning = false;

    private Action onTransitionComplete; // ������� ����� ���������� ��������

    private void Start()
    {
        // �������� ������ ������ � �������� Canvas
        Canvas canvas = GetComponent<Canvas>();
        screenSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // ������������� ��������� ������� �������
        SetInitialPositions();

        // ��������� ��������������
        SetInteractable(false);

        // ��������� �������� ��� �������� ������
        StartCoroutine(StartForwardTransition());
    }

    private void SetInitialPositions()
    {
        // ������������� FirstPanel � �����
        firstPanel.anchoredPosition = Vector2.zero;

        // ������������� SecondPanel �� ������ ���� ������
        secondPanel.anchoredPosition = new Vector2(screenSize.x, 0);
    }

    private IEnumerator StartForwardTransition()
    {
        // ���� �������� ��������
        yield return new WaitForSeconds(delayBeforeTransition);

        // ��������� ������������ ����������� �������
        StartCoroutine(MovePanel(firstPanel, new Vector2(-screenSize.x, 0), transitionDuration));
        StartCoroutine(MovePanel(secondPanel, Vector2.zero, transitionDuration));

        // ���� ���������� ��������
        yield return new WaitForSeconds(transitionDuration);

        // ��������� FirstPanel
        firstPanel.gameObject.SetActive(false);

        // ��������� �������������� � SecondPanel
        SetInteractable(true);
    }

    public void StartBackwardTransition(Action onComplete = null)
    {
        if (isTransitioning)
            return;

        isTransitioning = true;
        onTransitionComplete = onComplete;

        // �������� FirstPanel
        firstPanel.gameObject.SetActive(true);

        // ������������� ������� ������� ����� �������� ���������
        firstPanel.anchoredPosition = new Vector2(-screenSize.x, 0);
        secondPanel.anchoredPosition = Vector2.zero;

        // ��������� ��������������
        SetInteractable(false);

        // ��������� �������� ��� ��������� ��������
        StartCoroutine(BackwardTransition());
    }

    private IEnumerator BackwardTransition()
    {
        // ��������� ������������ ����������� �������
        StartCoroutine(MovePanel(firstPanel, Vector2.zero, transitionDuration));
        StartCoroutine(MovePanel(secondPanel, new Vector2(screenSize.x, 0), transitionDuration));

        // ���� ���������� ��������
        yield return new WaitForSeconds(transitionDuration);

        // ��������� SecondPanel
        secondPanel.gameObject.SetActive(false);

        // ��������� �������������� � FirstPanel
        SetInteractable(true);

        isTransitioning = false;

        // �������� ������� ����� ���������� ��������
        onTransitionComplete?.Invoke();
    }

    private IEnumerator MovePanel(RectTransform panel, Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, smoothT);
            yield return null;
        }

        // ������������� ������ �������� �������
        panel.anchoredPosition = targetPosition;
    }

    private void SetInteractable(bool value)
    {
        // ������������� interactable � blocksRaycasts ��� �������
        CanvasGroup firstCanvasGroup = firstPanel.GetComponent<CanvasGroup>();
        CanvasGroup secondCanvasGroup = secondPanel.GetComponent<CanvasGroup>();

        if (firstCanvasGroup != null)
        {
            firstCanvasGroup.interactable = value;
            firstCanvasGroup.blocksRaycasts = value;
        }

        if (secondCanvasGroup != null)
        {
            secondCanvasGroup.interactable = value;
            secondCanvasGroup.blocksRaycasts = value;
        }
    }
}
