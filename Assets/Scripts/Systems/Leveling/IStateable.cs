using UnityEngine;

namespace Systems.Leveling
{
    public interface IStateable
    {
        public StateOption[] Options { get; }

        public void AcceptState(int stateId);

        public void SetOptionsFrom(int stateId, Transform target);
        public bool TryGetOption(int stateId, out StateOption option);
    }
}