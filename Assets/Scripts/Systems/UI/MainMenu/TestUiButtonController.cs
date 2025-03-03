using System.Collections.Generic;
using Systems.Core.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.MainMenu
{
    public class TestUiButtonController : MonoBehaviour
    {
        public List<Button> Buttons;

        private LevelLoader _loader;

        private void Awake()
        {
            _loader = FindObjectOfType<LevelLoader>();

            Buttons[0].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[0]));
        }
    }
}
