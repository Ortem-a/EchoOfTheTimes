using Systems.Inputs;
using UnityEngine;
using Zenject;

namespace Systems.UI.Level
{
    public sealed class LevelStateUiButton : UiButton
    {
        [SerializeField]
        private int _stateId;

        private UserInputAdapter _inputAdapter;

        [Inject]
        private void Construct(UserInputAdapter inputAdapter)
        {
            _inputAdapter = inputAdapter;
        }

        protected override void HandleButtonClicked()
        {
            // add on click animation

            _inputAdapter.SwitchState(_stateId);
        }
    }
}