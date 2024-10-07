using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeableUI : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.7f; // Время плавного исчезновения

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("Компонент CanvasGroup не найден на объекте " + gameObject.name + ". Пожалуйста, добавьте компонент CanvasGroup на этот объект.");
        }
    }

    public void StartFadeOut()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeOutCoroutine());
        }
        else
        {
            Debug.LogError("Невозможно запустить FadeOutCoroutine, так как canvasGroup равно null на объекте " + gameObject.name);
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        float originalAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(originalAlpha, 0f, Mathf.SmoothStep(0f, 1f, t));
            canvasGroup.alpha = alpha;
            yield return null;
        }

        // Устанавливаем альфа в 0 и отключаем объект
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
