using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeableUI : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f; // Время плавного появления и исчезновения

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Устанавливаем начальную прозрачность в 0, чтобы объект был полностью прозрачным при старте
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        // Запускаем плавное появление объекта при старте
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        StopAllCoroutines(); // Останавливаем все корутины, чтобы избежать конфликтов
        StartCoroutine(FadeInCoroutine());
    }

    public void StartFadeOut()
    {
        StopAllCoroutines(); // Останавливаем все корутины, чтобы избежать конфликтов
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha; // Должно быть 0
        float targetAlpha = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Устанавливаем окончательное значение альфа
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha; // Может быть 1 или текущее значение
        float targetAlpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Устанавливаем окончательное значение альфа

        // Отключаем объект после исчезновения, если это необходимо
        // Если не хотите отключать объект, закомментируйте следующую строку
        gameObject.SetActive(false);
    }
}
