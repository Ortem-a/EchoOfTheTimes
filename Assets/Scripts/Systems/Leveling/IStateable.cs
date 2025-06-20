using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

namespace Systems.Leveling
{
    public interface IStateable
    {
        public SerializedDictionary<int, StateOption> Options { get; }

        public void AcceptState(int stateId, Action onComplete);
        public void SetOptionsFrom(int stateId, Transform target);
        public bool TryGetOption(int stateId, out StateOption option);
        public bool TryAcceptStateImmediate(int stateId);
    }
}