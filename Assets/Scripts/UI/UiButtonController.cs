using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    public class UiButtonController : MonoBehaviour
    {
        private UserInputHandler _userInputHandler;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Initialize(int stateId)
        {
            _userInputHandler = GameManager.Instance.UserInputHandler;

            _button.transform.GetChild(0).GetComponent<TMP_Text>().text = $"To {stateId}";
            _button.onClick.AddListener(() => ChangeState(stateId));
        }

        private void ChangeState(int stateId) 
        {
            _userInputHandler.ChangeLevelState(stateId);
        }
    }
}