using UnityEngine;
using System.Collections;

public class ScreenTransitionManager : MonoBehaviour
{
    public RectTransform firstPanel;
    public RectTransform secondPanel;
    public float transitionDuration = 1f; // Время, за которое панели перемещаются
    public float delayBeforeTransition = 2f; // Задержка перед началом перехода

    private Vector2 screenSize;

    private void Start()
    {
        // Получаем размер экрана в единицах Canvas
        Canvas canvas = GetComponent<Canvas>();
        screenSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // Устанавливаем начальные позиции панелей
        SetInitialPositions();

        // Запускаем корутину для перехода
        StartCoroutine(StartTransition());
    }

    private void SetInitialPositions()
    {
        // Устанавливаем FirstPanel в центр
        firstPanel.anchoredPosition = Vector2.zero;

        // Устанавливаем SecondPanel за правый край экрана
        secondPanel.anchoredPosition = new Vector2(screenSize.x, 0);
    }

    private IEnumerator StartTransition()
    {
        // Ждем заданную задержку
        yield return new WaitForSeconds(delayBeforeTransition);

        // Запускаем параллельное перемещение панелей
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

        // Устанавливаем точную конечную позицию
        panel.anchoredPosition = targetPosition;
    }
}
