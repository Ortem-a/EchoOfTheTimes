using EchoOfTheTimes.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    public class UiStateButton : MonoBehaviour
    {
        private InputMediator _inputMediator;
        private Button _button;

        public void Init(int stateId, InputMediator inputHandler)
        {
            _inputMediator = inputHandler;

            _button = GetComponent<Button>();

            _button.transform.GetChild(0).GetComponent<TMP_Text>().text = $"To {stateId}";
            _button.onClick.AddListener(() => ChangeState(stateId));
        }

        private void ChangeState(int stateId)
        {
            _inputMediator.ChangeLevelState(stateId);
        }
    }
}