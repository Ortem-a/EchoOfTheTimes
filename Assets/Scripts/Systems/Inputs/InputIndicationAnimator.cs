using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.Inputs
{
    public abstract class InputIndicationAnimator : MonoBehaviour
    {
        protected GameObject spawnedIndicator;
        protected InputIndicatorSettingsScriptableObject inputIndicatorSettings;

        [Inject]
        private void Construct(InputIndicatorSettingsScriptableObject inputIndicatorSettings)
        {
            this.inputIndicatorSettings = inputIndicatorSettings;
        }

        protected abstract void Awake();
    }
}