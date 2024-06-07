using System.Collections.Generic;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<IStateParameter> StatesParameters;
    }
}