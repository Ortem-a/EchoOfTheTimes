using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChapterTransitionManager : MonoBehaviour
{
    [Header("Настройки перехода")]
    public float fadeDuration = 1f; // Время исчезновения UI элементов

    private bool isTransitioning = false;
    private List<FadeableUI> fadeableUIElements;
    private CanvasGroup[] canvasGroups;

    private void Start()
    {
        // Находим все UI-элементы с FadeableUI
        fadeableUIElements = new List<FadeableUI>(FindObjectsOfType<FadeableUI>());

        // Находим все CanvasGroup для блокировки взаимодействия
        canvasGroups = FindObjectsOfType<CanvasGroup>();
    }

    public void StartTransition(string sceneName)
    {
        if (isTransitioning)
            return;

        isTransitioning = true;

        // Запускаем корутину перехода
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // Блокируем взаимодействие с интерфейсом
        DisableUIInteraction();

        // Плавно скрываем UI-элементы
        yield return StartCoroutine(FadeOutUIElements());

        // Загружаем целевую сцену
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void DisableUIInteraction()
    {
        // Блокируем взаимодействие с интерфейсом
        foreach (CanvasGroup cg in canvasGroups)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    private IEnumerator FadeOutUIElements()
    {
        // Запускаем плавное исчезновение всех FadeableUI элементов
        foreach (FadeableUI fadeable in fadeableUIElements)
        {
            if (fadeable != null)
            {
                fadeable.StartFadeOut();
            }
        }

        // Определяем максимальное время исчезновения
        float maxFadeDuration = 0f;
        foreach (FadeableUI fadeable in fadeableUIElements)
        {
            if (fadeable != null && fadeable.fadeDuration > maxFadeDuration)
            {
                maxFadeDuration = fadeable.fadeDuration;
            }
        }

        // Ждем, пока все элементы полностью исчезнут
        yield return new WaitForSeconds(maxFadeDuration);
    }
}
