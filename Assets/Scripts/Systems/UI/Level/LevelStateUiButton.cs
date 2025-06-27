using Systems.Inputs;
using UnityEngine;
using Zenject;

namespace Systems.UI.Level
{
    [RequireComponent(typeof(UiRadioButtonAnimator))]
    public sealed class LevelStateUiButton : UiButton
    {
        [field: SerializeField]
        public int StateId { get; private set; }

        private StateRadioButtonController _radioButtonController;
        private UserInputAdapter _inputAdapter;
        public UiRadioButtonAnimator Animator { get; private set; }

        [Inject]
        private void Construct(UserInputAdapter inputAdapter, StateRadioButtonController radioButtonController)
        {
            _inputAdapter = inputAdapter;
            _radioButtonController = radioButtonController;

            Animator = GetComponent<UiRadioButtonAnimator>();
        }

        protected override void HandleButtonClicked()
        {
            // add on click animation
            _radioButtonController.SwitchToButton(StateId);

            _inputAdapter.SwitchState(StateId);
        }
    }
}