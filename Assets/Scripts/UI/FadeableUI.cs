using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeableUI : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.7f; // ����� �������� ������������

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("��������� CanvasGroup �� ������ �� ������� " + gameObject.name + ". ����������, �������� ��������� CanvasGroup �� ���� ������.");
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
            Debug.LogError("���������� ��������� FadeOutCoroutine, ��� ��� canvasGroup ����� null �� ������� " + gameObject.name);
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

        // ������������� ����� � 0 � ��������� ������
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
