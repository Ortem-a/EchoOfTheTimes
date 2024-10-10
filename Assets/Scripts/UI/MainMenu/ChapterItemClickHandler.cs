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

        [Header("��������� �����")]
        public string sceneName; // �������� ����� ��� ��������
        public float delay = 1f; // ����� �������� ����� ��������� ����� (������ ��������� � fadeDuration � FadeableUI)

        [Header("Canvas ��� ����������")]
        public RectTransform firstCanvas; // ������ Canvas ��� ����������
        public RectTransform secondCanvas; // ������ Canvas ��� ����������
        public float scaleDuration = 1f; // ����� ����� ���������� � ���������� Canvas
        public float targetScale = 1.1f; // ������������ ����������

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

                // ��������� ���������� � ���������� ��������
                StartCoroutine(ScaleCanvasOverTime(firstCanvas, secondCanvas, targetScale, scaleDuration));

                // ������� ��� ������� � FadeableUI � ��������� �� ������� ������������
                FadeableUI[] fadeableObjects = FindObjectsOfType<FadeableUI>();
                foreach (FadeableUI fadeable in fadeableObjects)
                {
                    if (fadeable != null)
                    {
                        fadeable.StartFadeOut();
                    }
                }

                // ��������� �������������� � �����������
                DisableUIInteraction();

                // ��������� �������� ��� �������� ����� � ���������
                StartCoroutine(LoadSceneAfterDelay());
            }
            else
            {
                Debug.LogError("�������� ����� �� ������� � ChapterItemClickHandler �� ������� " + gameObject.name, this);
            }
        }

        // ��������� ��� ����������, � ����� ���������� Canvas
        private IEnumerator ScaleCanvasOverTime(RectTransform canvas1, RectTransform canvas2, float targetScale, float duration)
        {
            Vector2 originalSize1 = canvas1.sizeDelta;
            Vector2 originalSize2 = canvas2.sizeDelta;
            float halfDuration = duration / 2f; // �������� ������� �� ���������� � �������� �� ����������
            float elapsedTime = 0f;

            // ���� 1: ����������
            while (elapsedTime < halfDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / halfDuration;
                float scaleFactor = Mathf.Lerp(1f, targetScale, Mathf.SmoothStep(0f, 1f, t));

                // ������ ������ ��������
                canvas1.sizeDelta = originalSize1 * scaleFactor;
                canvas2.sizeDelta = originalSize2 * scaleFactor;

                yield return null;
            }

            // ���� 2: ���������� �� ��������� �������
            elapsedTime = 0f;
            while (elapsedTime < halfDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / halfDuration;
                float scaleFactor = Mathf.Lerp(targetScale, 1f, Mathf.SmoothStep(0f, 1f, t));

                // ������ ������ ��������
                canvas1.sizeDelta = originalSize1 * scaleFactor;
                canvas2.sizeDelta = originalSize2 * scaleFactor;

                yield return null;
            }

            // ��������������� ������������ ������ ��������
            canvas1.sizeDelta = originalSize1;
            canvas2.sizeDelta = originalSize2;
        }

        private IEnumerator LoadSceneAfterDelay()
        {
            // ���� �������� �����
            yield return new WaitForSeconds(delay);

            // ��������� �����
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("0_MainMenu_UI_Chapters");
        }

        private void DisableUIInteraction()
        {
            // ������� ��� CanvasGroup �� ����� � ��������� ��������������
            CanvasGroup[] canvasGroups = FindObjectsOfType<CanvasGroup>();
            foreach (CanvasGroup cg in canvasGroups)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }
    }
}