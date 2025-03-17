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
            Buttons[1].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[1]));
            Buttons[2].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[2]));
            Buttons[3].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[3]));
            Buttons[4].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[4]));
            Buttons[5].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[5]));
            Buttons[6].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[6]));
            Buttons[7].onClick.AddListener(
                async () => await _loader.LoadLevelAsync(_loader.Chapters[1].Levels[7]));
        }
    }
}
