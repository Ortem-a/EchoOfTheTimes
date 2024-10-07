using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ButtonSceneLoader : MonoBehaviour, IPointerClickHandler
{
    [Header("���������")]
    public string sceneName; // �������� ����� ��� ��������
    public float delay = 0.1f; // �������� ����� ��������� ����� (������ ��������� � ������������ fadeDuration ����� ���� FadeableUI)

    public void OnPointerClick(PointerEventData eventData)
    {
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
