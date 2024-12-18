using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Leveling
{
    public class Stateable : MonoBehaviour, IStateable
    {
        [SerializeField]
        private Dictionary<int, StateOption> _options = new Dictionary<int, StateOption>();

        public Dictionary<int, StateOption> Options => _options;

        public void AcceptState(int stateId)
        {
            var option = Options[stateId];

            throw new NotImplementedException();
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
    }
}