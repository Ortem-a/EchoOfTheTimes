using UnityEngine;

namespace EchoOfTheTimes.Persistence
{
    [System.Serializable]
    public class PlayerData
    {
        public Vector3 Checkpoint;
        public int StateId;

        public override string ToString()
        {
            return $"StateId {StateId} | Checkpoint {Checkpoint}";
        }
    }
}