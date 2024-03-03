using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class SpecialTransition
    {
        public int StateFromId;
        public int StateToId;

        public List<Stateable> Influenced;
    }
}