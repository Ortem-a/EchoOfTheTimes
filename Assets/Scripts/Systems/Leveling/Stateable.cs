using System.Collections.Generic;
using Systems.Tools;
using UnityEngine;

namespace Systems.Leveling
{
    [RequireComponent(typeof(MarkerParent))]
    public class Stateable : MonoBehaviour, IStateable
    {
        public StateableSerializableDictionary SerializableDictionary;

        private Dictionary<int, StateOption> _options;

        public Dictionary<int, StateOption> Options => _options;

        private void Construct()
        {
            // simulate entry point
            _options = SerializableDictionary.ToDictionary();
        }

        public void AcceptState(int stateId)
        {
            var option = Options[stateId];

            AcceptState(option);
        }

        public void SetOptionsFrom(int stateId, Transform target)
        {
            _options ??= SerializableDictionary.ToDictionary();//new Dictionary<int, StateOption>();

            var newOption = new StateOption()
            {
                Target = target,
                LocalPosition = target.localPosition,
                LocalRotation = target.localRotation,
                LocalScale = target.localScale,
            };

            if (Options.ContainsKey(stateId))
            {
                Options[stateId] = newOption;
            }
            else
            {
                Options.Add(stateId, newOption);
            }
        }

        public bool TryGetOption(int stateId, out StateOption option)
        {
            if (Options.TryGetValue(stateId, out var opt))
            {
                option = opt;
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