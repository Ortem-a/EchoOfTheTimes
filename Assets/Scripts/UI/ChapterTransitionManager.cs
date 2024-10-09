using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChapterTransitionManager : MonoBehaviour
{
    [Header("��������� ��������")]
    public float fadeDuration = 1f; // ����� ������������ UI ���������

    private bool isTransitioning = false;
    private List<FadeableUI> fadeableUIElements;
    private CanvasGroup[] canvasGroups;

    private void Start()
    {
        // ������� ��� UI-�������� � FadeableUI
        fadeableUIElements = new List<FadeableUI>(FindObjectsOfType<FadeableUI>());

        // ������� ��� CanvasGroup ��� ���������� ��������������
        canvasGroups = FindObjectsOfType<CanvasGroup>();
    }

    public void StartTransition(string sceneName)
    {
        if (isTransitioning)
            return;

        isTransitioning = true;

        // ��������� �������� ��������
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // ��������� �������������� � �����������
        DisableUIInteraction();

        // ������ �������� UI-��������
        yield return StartCoroutine(FadeOutUIElements());

        // ��������� ������� �����
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void DisableUIInteraction()
    {
        // ��������� �������������� � �����������
        foreach (CanvasGroup cg in canvasGroups)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    private IEnumerator FadeOutUIElements()
    {
        // ��������� ������� ������������ ���� FadeableUI ���������
        foreach (FadeableUI fadeable in fadeableUIElements)
        {
            if (fadeable != null)
            {
                fadeable.StartFadeOut();
            }
        }

        // ���������� ������������ ����� ������������
        float maxFadeDuration = 0f;
        foreach (FadeableUI fadeable in fadeableUIElements)
        {
            if (fadeable != null && fadeable.fadeDuration > maxFadeDuration)
            {
                maxFadeDuration = fadeable.fadeDuration;
            }
        }

        // ����, ���� ��� �������� ��������� ��������
        yield return new WaitForSeconds(maxFadeDuration);
    }
}
