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

            //if (Options.ContainsKey(stateId))
            //{
            //    Options[stateId] = newOption;
            //}
            //else
            //{
            //    Options.Add(stateId, newOption);
            //}

            if (Options.Length != 0 && stateId < Options.Length)
            {
                Options[stateId] = newOption;
            }
            else
            {
                int newSize = Options.Length + (stateId - Options.Length + 1);
                var newOptions = new StateOption[newSize];
                Array.Copy(Options, newOptions, Options.Length);

                // fill with defaults
                int fromIndex = Options.Length;
                int length = stateId - Options.Length;
                for (int i = 0; i < length; i++)
                {
                    newOptions[fromIndex + i] = new StateOption()
                    {
                        Target = target,
                        LocalPosition = Vector3.zero,
                        LocalRotation = Quaternion.identity,
                        LocalScale = Vector3.one
                    };
                }

                newOptions[stateId] = newOption;
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

            Debug.LogError($"There is no option with key: <{stateId}>");

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