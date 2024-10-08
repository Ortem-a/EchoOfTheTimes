using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public ScreenTransitionManager transitionManager;
    public string sceneToLoadAfterTransition; // Название сцены для загрузки после перехода (если нужно)

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
            Debug.LogError("BackButtonHandler должен быть прикреплен к объекту с компонентом Button.");
        }
    }

    private void OnBackButtonClicked()
    {
        if (transitionManager != null)
        {
            // Запускаем обратный переход и загружаем сцену после его завершения
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
            Debug.LogError("TransitionManager не установлен в BackButtonHandler.");
        }
    }
}
