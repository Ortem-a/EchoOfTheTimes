using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI
{
    public class UiButtonController : MonoBehaviour
    {
        private InputMediator _userInputHandler;
        private Button _button;

        public void Initialize(int stateId, InputMediator inputHandler)
        {
            _userInputHandler = inputHandler;

            _button = GetComponent<Button>();

            _button.transform.GetChild(0).GetComponent<TMP_Text>().text = $"To {stateId}";
            _button.onClick.AddListener(() => ChangeState(stateId));
        }

        private void ChangeState(int stateId) 
        {
            _userInputHandler.ChangeLevelState(stateId);
        }
    }
}