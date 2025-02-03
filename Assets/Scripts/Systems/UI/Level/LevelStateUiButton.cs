using Systems.Inputs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Systems.UI.Level
{
    public class LevelStateUiButton : MonoBehaviour
    {
        [SerializeField]
        private int _stateId;

        private UserInputAdapter _inputAdapter;
        private Button _button;

        [Inject]
        private void Construct(UserInputAdapter inputAdapter)
        {
            _inputAdapter = inputAdapter;

            _button = GetComponent<Button>();
            _button.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            _inputAdapter.SwitchState(_stateId);
        }
    }
}