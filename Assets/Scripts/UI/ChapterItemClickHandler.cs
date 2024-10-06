using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChapterItemClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("��������� �����")]
    public string sceneName; // �������� ����� ��� ��������
    public float delay = 1f; // ����� �������� ����� ��������� ����� (������ ��������� � fadeDuration � FadeableUI)

    private bool isTransitioning = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTransitioning)
            return;

        if (!string.IsNullOrEmpty(sceneName))
        {
            isTransitioning = true;

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
            Debug.LogError("�������� ����� �� ������� � ChapterItemClickHandler �� ������� " + gameObject.name);
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // ���� �������� �����
        yield return new WaitForSeconds(delay);

        // ��������� �����
        SceneManager.LoadScene(sceneName);
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
