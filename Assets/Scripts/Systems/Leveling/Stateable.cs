using System;
using UnityEngine;

namespace Systems.Leveling
{
    [RequireComponent(typeof(MarkerParent))]
    public class Stateable : MonoBehaviour, IStateable
    {
        [SerializeField]
        private StateOption[] _options = Array.Empty<StateOption>();
        public StateOption[] Options => _options;

        public void AcceptState(int stateId)
        {
            var option = Options[stateId];

            AcceptState(option);
        }

        public void SetOptionsFrom(int stateId, Transform target)
        {
            var newOption = new StateOption()
            {
                Target = target,
                LocalPosition = target.localPosition,
                LocalRotation = target.localRotation,
                LocalScale = target.localScale,
            };

            if (stateId < Options.Length)
            {
                Options[stateId] = newOption;
            }
            else
            {
                // add new element
                var newOptions = new StateOption[Options.Length + 1];
                Array.Copy(Options, newOptions, Options.Length);
                newOptions[^1] = newOption;
                _options = newOptions;
            }
        }

        public bool TryGetOption(int stateId, out StateOption option)
        {
            if (Options.Length != 0 && stateId < Options.Length)
            {
                option = Options[stateId];
                return true;
            }

            Debug.LogError($"There is no option with ID: <{stateId}>");

            option = null;
            return false;
        }

        private void AcceptState(StateOption option)
        {
            option.Target.SetLocalPositionAndRotation(
                option.LocalPosition, option.LocalRotation
                );
            option.Target.localScale = option.LocalScale;
        }
    }
}