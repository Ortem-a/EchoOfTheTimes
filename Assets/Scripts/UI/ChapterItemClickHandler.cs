using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChapterItemClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("Настройки главы")]
    public string sceneName; // Название сцены для загрузки
    public float delay = 1f; // Время задержки перед загрузкой сцены (должно совпадать с fadeDuration в FadeableUI)

    private bool isTransitioning = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTransitioning)
            return;

        if (!string.IsNullOrEmpty(sceneName))
        {
            isTransitioning = true;

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
        else
        {
            Debug.LogError("Название сцены не указано в ChapterItemClickHandler на объекте " + gameObject.name);
        }
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
