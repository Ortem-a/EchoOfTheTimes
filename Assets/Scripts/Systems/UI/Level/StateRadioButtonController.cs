using UnityEngine;

namespace Systems.UI.Level
{
    /// <summary>
    /// implement radio button logic for UI state buttons 
    /// - run animation for each of them
    /// </summary>
    public sealed class StateRadioButtonController : MonoBehaviour
    {
        private LevelStateUiButton[] _stateButtons;

        private int _lastPressedButtonId = 0;

        private void Awake()
        {
            _stateButtons = GetComponentsInChildren<LevelStateUiButton>();
        }

        private void Start()
        {
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            _stateButtons[0].Animator.PlayEnableAnimation();

            for (int i = 1; i < _stateButtons.Length; i++)
            {
                _stateButtons[i].Animator.PlayDisableAnimation();
            }
        }

        public void SwitchToButton(int stateId)
        {
            // skip if clicked on same button again
            if (_lastPressedButtonId == stateId) return;

            // activate pressed button
            // disable previous button
            for (int i = 0; i < _stateButtons.Length; i++)
            {
                if (_stateButtons[i].StateId == stateId)
                {
                    _stateButtons[i].Animator.PlayEnableAnimation();
                    continue;
                }
                
                if (_stateButtons[i].StateId == _lastPressedButtonId)
                {
                    _stateButtons[i].Animator.PlayDisableAnimation();
                    continue;
                }
            }

            _lastPressedButtonId = stateId;
        }
    }
}