using EchoOfTheTimes.Interfaces;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class StateParameter : MonoBehaviour, IGotState
    {
        public Transform Target => transform;
        public Vector3 Position => transform.position;
        public Vector3 Rotation => transform.rotation.eulerAngles;
        public Vector3 LocalScale => transform.localScale;
    }
}