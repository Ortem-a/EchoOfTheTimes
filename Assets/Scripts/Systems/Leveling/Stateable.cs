using System.Collections.Generic;
using Systems.Tools;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    [RequireComponent(typeof(MarkerParent))]
    public class Stateable : MonoBehaviour, IStateable
    {
        private Dictionary<int, StateOption> _options;

        public Dictionary<int, StateOption> Options => _options;

        public StateableSerializableDictionary States;

        //[SerializeField]
        //private StateOption[] _options = Array.Empty<StateOption>();

        //public StateOption[] Options => _options;

        public void Initialize()
        {
            _options = States.ToDictionary();
        }

        public void AcceptState(int stateId)
        {
            if (Options.TryGetValue(stateId, out var option))
            {
                AcceptState(option);
            }

            //var option = Options[stateId];

            //AcceptState(option);
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

            States.AddOrUpdate(stateId, target);

            //if (Options.ContainsKey(stateId))
            //{
            //    Options[stateId] = newOption;
            //}
            //else
            //{
            //    Options.Add(stateId, newOption);
            //}

            //if (Options.Length != 0 && stateId < Options.Length)
            //{
            //    Options[stateId] = newOption;
            //}
            //else
            //{
            //    int newSize = Options.Length + (stateId - Options.Length + 1);
            //    var newOptions = new StateOption[newSize];
            //    Array.Copy(Options, newOptions, Options.Length);

            //    // fill with defaults
            //    int fromIndex = Options.Length;
            //    int length = stateId - Options.Length;
            //    for (int i = 0; i < length; i++)
            //    {
            //        newOptions[fromIndex + i] = new StateOption()
            //        {
            //            Target = target,
            //            LocalPosition = Vector3.zero,
            //            LocalRotation = Quaternion.identity,
            //            LocalScale = Vector3.one
            //        };
            //    }

            //    newOptions[stateId] = newOption;
            //    _options = newOptions;
            //}
        }

        public bool TryGetOption(int stateId, out StateOption option)
        {
            if (States.TryGetValue(stateId, out var newOption))
            {
                option = newOption;
                return true;
            }

            //if (Options.Length != 0 && stateId < Options.Length)
            //{
            //    option = Options[stateId];
            //    return true;
            //}

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