using EchoOfTheTimes.Core;
using EchoOfTheTimes.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
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