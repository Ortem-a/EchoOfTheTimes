using System.Collections.Generic;
using UnityEngine;

namespace Systems.Leveling
{
    public interface IStateable
    {
        public Dictionary<int, StateOption> Options { get; }

        public void AcceptState(int stateId);

        public void SetOptionsFrom(int stateId, Transform target);
        public bool TryGetOption(int stateId, out StateOption option);
    }
}