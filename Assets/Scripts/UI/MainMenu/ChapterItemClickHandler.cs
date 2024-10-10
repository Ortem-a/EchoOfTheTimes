using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterItemClickHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private LevelStatusUpdater _levelsPanel;
        
        private GameObject _chaptersPanel;
        private GameObject _chaptersFooterPanel;

        [Header("Настройки главы")]
        public string sceneName; // Название сцены для загрузки
        public float delay = 1f; // Время задержки перед загрузкой сцены (должно совпадать с fadeDuration в FadeableUI)

        [Header("Canvas для увеличения")]
        public RectTransform firstCanvas; // Первый Canvas для увеличения
        public RectTransform secondCanvas; // Второй Canvas для увеличения
        public float scaleDuration = 1f; // Общее время увеличения и уменьшения Canvas
        public float targetScale = 1.1f; // Максимальное увеличение

        private bool isTransitioning = false;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _chaptersPanel = mainMenuService.ChaptersPanel;
            _chaptersFooterPanel = mainMenuService.ChaptersFooterPanel;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _chaptersPanel.SetActive(false);
            _chaptersFooterPanel.SetActive(false);
            _levelsPanel.transform.parent.gameObject.SetActive(true);

            return;

            if (isTransitioning)
                return;

            if (!string.IsNullOrEmpty(sceneName))
            {
                isTransitioning = true;

                // Запускаем увеличение и уменьшение канвасов
                StartCoroutine(ScaleCanvasOverTime(firstCanvas, secondCanvas, targetScale, scaleDuration));

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
                Debug.LogError("Название сцены не указано в ChapterItemClickHandler на объекте " + gameObject.name, this);
            }
        }

        // Коррутина для увеличения, а затем уменьшения Canvas
        private IEnumerator ScaleCanvasOverTime(RectTransform canvas1, RectTransform canvas2, float targetScale, float duration)
        {
            Vector2 originalSize1 = canvas1.sizeDelta;
            Vector2 originalSize2 = canvas2.sizeDelta;
            float halfDuration = duration / 2f; // Половина времени на увеличение и половина на уменьшение
            float elapsedTime = 0f;

            // Фаза 1: Увеличение
            while (elapsedTime < halfDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / halfDuration;
                float scaleFactor = Mathf.Lerp(1f, targetScale, Mathf.SmoothStep(0f, 1f, t));

                // Меняем размер канвасов
                canvas1.sizeDelta = originalSize1 * scaleFactor;
                canvas2.sizeDelta = originalSize2 * scaleFactor;

                yield return null;
            }

            // Фаза 2: Уменьшение до исходного размера
            elapsedTime = 0f;
            while (elapsedTime < halfDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / halfDuration;
                float scaleFactor = Mathf.Lerp(targetScale, 1f, Mathf.SmoothStep(0f, 1f, t));

                // Меняем размер канвасов
                canvas1.sizeDelta = originalSize1 * scaleFactor;
                canvas2.sizeDelta = originalSize2 * scaleFactor;

                yield return null;
            }

            // Восстанавливаем оригинальный размер канвасов
            canvas1.sizeDelta = originalSize1;
            canvas2.sizeDelta = originalSize2;
        }

        private IEnumerator LoadSceneAfterDelay()
        {
            // Ждем заданное время
            yield return new WaitForSeconds(delay);

            // Загружаем сцену
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("0_MainMenu_UI_Chapters");
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
}