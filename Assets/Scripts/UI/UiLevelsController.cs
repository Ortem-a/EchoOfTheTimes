using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI
{
    public class UiLevelsController : MonoBehaviour
    {
        public Transform ButtonsContainer;
        private Button[] _buttons;

        private SceneLoader _loader;

        private void Start()
        {
            _loader = FindObjectOfType<SceneLoader>();
            _buttons = ButtonsContainer.GetComponentsInChildren<Button>();

            for (int i = 1; i < _loader.SceneGroups.Length; i++)
            {
                _buttons[i - 1].transform.GetChild(0).GetComponent<TMP_Text>().text = _loader.SceneGroups[i].GroupName;
            }
        }

        public void LoadSceneGroup(int index)
        {
            _loader.LoadSceneGroupAsync(index);
        }
    }
}