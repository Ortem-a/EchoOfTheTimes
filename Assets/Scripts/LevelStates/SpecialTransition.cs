using System.Collections.Generic;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class SpecialTransition
    {
        public int StateFromId;
        public int StateToId;

        public List<Stateable> Influenced;

        public bool EqualsWith(Transition transition)
        {
            return transition.StateFromId == StateFromId && transition.StateToId == StateToId;
        }
    }
}