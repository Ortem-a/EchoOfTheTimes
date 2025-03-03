using Systems.Core.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Level
{
    public class ExitLevelUiButton : MonoBehaviour
    {
        private Button _button;
        private LevelLoader _loader;

        private void Awake()
        {
            _loader = FindObjectOfType<LevelLoader>();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnExitLevel);
        }

        private async void OnExitLevel()
        {
            await _loader.LoadMainMenuSceneAsync();
        }
    }
}
