#if UNITY_EDITOR
using EchoOfTheTimes.Editor;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(StairsCreator))]
    public class StateableStair : MonoBehaviour
    {
        private enum StairType
        {
            FlatDown,
            UpHill,
            DownHill,
            FlatTop
        }

        private StairType _type;
    }
}
#endif