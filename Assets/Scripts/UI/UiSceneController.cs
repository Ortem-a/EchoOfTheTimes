using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    public class UiSceneController : MonoBehaviour
    {
        public Button ToMainMenuButton;
        public Transform BottomPanel;
        public GameObject ButtonPrefab;
        public TMP_Text InfoLabel;

        public int StatesNumber;

        private SceneLoader _loader;

        private void Awake()
        {
            ToMainMenuButton.onClick.AddListener(ExitToMainMenu);
        }

        private void Start()
        {
            _loader = FindObjectOfType<SceneLoader>();

            for (int i = 0; i < StatesNumber; i++)
            {
                var obj = Instantiate(ButtonPrefab, BottomPanel);
                obj.GetComponent<UiButtonController>().Initialize(i);
            }
        }

        private void ExitToMainMenu()
        {
            _loader.LoadSceneGroupAsync(0);
        }

        public void UpdateLabel(int stateId)
        {
            InfoLabel.text = $"State: {stateId}";
        }
    }
}