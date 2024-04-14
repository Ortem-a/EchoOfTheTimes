using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<StateParameter> StatesParameters;
    }
}