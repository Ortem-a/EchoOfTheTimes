using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Utils;
using UnityEngine;

namespace EchoOfTheTimes.Persistence
{
    [System.Serializable]
    public class PlayerData : ISaveable
    {
        [field: SerializeField]
        public SerializableGuid Id { get; set; }

        public Vector3 Checkpoint;
        public int StateId;

        public override string ToString()
        {
            return $"Id: {Id} | StateId {StateId} | Checkpoint {Checkpoint}";
        }
    }
}