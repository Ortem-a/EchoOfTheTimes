using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class StateParameter
    {
        public int StateId;
        public Transform Target;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 LocalScale;
    }
}