using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public ScreenTransitionManager transitionManager;
    public string sceneToLoadAfterTransition; // �������� ����� ��� �������� ����� �������� (���� �����)

    private Button backButton;

    private void Start()
    {
        backButton = GetComponent<Button>();
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        else
        {
            Debug.LogError("BackButtonHandler ������ ���� ���������� � ������� � ����������� Button.");
        }
    }

    private void OnBackButtonClicked()
    {
        if (transitionManager != null)
        {
            // ��������� �������� ������� � ��������� ����� ����� ��� ����������
            transitionManager.StartBackwardTransition(() =>
            {
                if (!string.IsNullOrEmpty(sceneToLoadAfterTransition))
                {
                    SceneManager.LoadScene(sceneToLoadAfterTransition);
                }
            });
        }
        else
        {
            Debug.LogError("TransitionManager �� ���������� � BackButtonHandler.");
        }
    }
}
