using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeableUI : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f; // ����� �������� ��������� � ������������

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("��������� CanvasGroup �� ������ �� ������� " + gameObject.name + ". ����������, �������� ��������� CanvasGroup �� ���� ������.");
        }

        // ������������� ��������� ������������ � 0, ����� ������ ��� ��������� ���������� ��� ������
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        // ��������� ������� ��������� ������� ��� ������
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        if (canvasGroup != null)
        {
            StopAllCoroutines(); // ������������� ��� ��������, ����� �������� ����������
            StartCoroutine(FadeInCoroutine());
        }
        else
        {
            Debug.LogError("���������� ��������� FadeInCoroutine, ��� ��� canvasGroup ����� null �� ������� " + gameObject.name);
        }
    }

    public void StartFadeOut()
    {
        if (canvasGroup != null)
        {
            StopAllCoroutines(); // ������������� ��� ��������, ����� �������� ����������
            StartCoroutine(FadeOutCoroutine());
        }
        else
        {
            Debug.LogError("���������� ��������� FadeOutCoroutine, ��� ��� canvasGroup ����� null �� ������� " + gameObject.name);
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha; // ������ ���� 0
        float targetAlpha = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // ������������� ������������� �������� �����
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha; // ����� ���� 1 ��� ������� ��������
        float targetAlpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // ������������� ������������� �������� �����

        // ��������� ������ ����� ������������, ���� ��� ����������
        // ���� �� ������ ��������� ������, ��������������� ��������� ������
        gameObject.SetActive(false);
    }
}
