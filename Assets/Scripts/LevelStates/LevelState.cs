using System.Collections.Generic;
using UnityEditor;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<StateParameter> StatesParameters;

        public SceneAsset Scene;
    }
}