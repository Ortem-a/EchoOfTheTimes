using UnityEngine;

namespace Systems.UI.Level
{
    public sealed class UiRadioButtonAnimator : MonoBehaviour
    {
        // simple animator need states
        // he has to know 
        // - is it pressed
        // - is it unpressed
        // - is it AFK --> need to play IDLE animation

        public void PlayEnableAnimation()
        {
            throw new System.NotImplementedException();
        }

        public void PlayDisableAnimation()
        {
            throw new System.NotImplementedException();
        }
    }
}