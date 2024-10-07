using UnityEngine;
using System.Collections;
using System;

public class ScreenTransitionManager : MonoBehaviour
{
    public RectTransform firstPanel;
    public RectTransform secondPanel;
    public float transitionDuration = 1f; // Время перемещения панелей
    public float delayBeforeTransition = 2f; // Задержка перед началом перехода

    private Vector2 screenSize;
    private bool isTransitioning = false;

    private Action onTransitionComplete; // Коллбек после завершения перехода

    private void Start()
    {
        // Получаем размер экрана в единицах Canvas
        Canvas canvas = GetComponent<Canvas>();
        screenSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // Устанавливаем начальные позиции панелей
        SetInitialPositions();

        // Блокируем взаимодействие
        SetInteractable(false);

        // Запускаем корутину для перехода вперед
        StartCoroutine(StartForwardTransition());
    }

    private void SetInitialPositions()
    {
        // Устанавливаем FirstPanel в центр
        firstPanel.anchoredPosition = Vector2.zero;

        // Устанавливаем SecondPanel за правый край экрана
        secondPanel.anchoredPosition = new Vector2(screenSize.x, 0);
    }

    private IEnumerator StartForwardTransition()
    {
        // Ждем заданную задержку
        yield return new WaitForSeconds(delayBeforeTransition);

        // Запускаем параллельное перемещение панелей
        StartCoroutine(MovePanel(firstPanel, new Vector2(-screenSize.x, 0), transitionDuration));
        StartCoroutine(MovePanel(secondPanel, Vector2.zero, transitionDuration));

        // Ждем завершения перехода
        yield return new WaitForSeconds(transitionDuration);

        // Отключаем FirstPanel
        firstPanel.gameObject.SetActive(false);

        // Разрешаем взаимодействие с SecondPanel
        SetInteractable(true);
    }

    public void StartBackwardTransition(Action onComplete = null)
    {
        if (isTransitioning)
            return;

        isTransitioning = true;
        onTransitionComplete = onComplete;

        // Включаем FirstPanel
        firstPanel.gameObject.SetActive(true);

        // Устанавливаем позиции панелей перед обратным переходом
        firstPanel.anchoredPosition = new Vector2(-screenSize.x, 0);
        secondPanel.anchoredPosition = Vector2.zero;

        // Блокируем взаимодействие
        SetInteractable(false);

        // Запускаем корутину для обратного перехода
        StartCoroutine(BackwardTransition());
    }

    private IEnumerator BackwardTransition()
    {
        // Запускаем параллельное перемещение панелей
        StartCoroutine(MovePanel(firstPanel, Vector2.zero, transitionDuration));
        StartCoroutine(MovePanel(secondPanel, new Vector2(screenSize.x, 0), transitionDuration));

        // Ждем завершения перехода
        yield return new WaitForSeconds(transitionDuration);

        // Отключаем SecondPanel
        secondPanel.gameObject.SetActive(false);

        // Разрешаем взаимодействие с FirstPanel
        SetInteractable(true);

        isTransitioning = false;

        // Вызываем коллбек после завершения перехода
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

        // Устанавливаем точную конечную позицию
        panel.anchoredPosition = targetPosition;
    }

    private void SetInteractable(bool value)
    {
        // Устанавливаем interactable и blocksRaycasts для панелей
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
