using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ButtonSceneLoader : MonoBehaviour, IPointerClickHandler
{
    [Header("Настройки")]
    public string sceneName; // Название сцены для загрузки
    public float delay = 0.1f; // Задержка перед загрузкой сцены (должна совпадать с максимальным fadeDuration среди всех FadeableUI)

    public void OnPointerClick(PointerEventData eventData)
    {
        // Находим все объекты с FadeableUI и запускаем их плавное исчезновение
        FadeableUI[] fadeableObjects = FindObjectsOfType<FadeableUI>();
        foreach (FadeableUI fadeable in fadeableObjects)
        {
            if (fadeable != null)
            {
                fadeable.StartFadeOut();
            }
        }

        // Блокируем взаимодействие с интерфейсом
        DisableUIInteraction();

        // Запускаем корутину для загрузки сцены с задержкой
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // Ждем заданное время
        yield return new WaitForSeconds(delay);

        // Загружаем сцену
        SceneManager.LoadScene(sceneName);
    }

    private void DisableUIInteraction()
    {
        // Находим все CanvasGroup на сцене и отключаем взаимодействие
        CanvasGroup[] canvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (CanvasGroup cg in canvasGroups)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
}
