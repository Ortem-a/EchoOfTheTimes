using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class BackButtonHandler : MonoBehaviour
    {
        public ScreenTransitionManager transitionManager;
        public string sceneToLoadAfterTransition;

        private Button backButton;

        private void Awake()
        {
            backButton = GetComponent<Button>();
            backButton.onClick.AddListener(OnBackButtonClicked);
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
}