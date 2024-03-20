using System.Collections.Generic;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class Transition
    {
        public int StateFromId;
        public int StateToId;

        public List<StateParameter> Parameters;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (Parameters != null)
            {
                foreach (var param in Parameters)
                {
                    sb.Append($"<{param}>, ");
                }
            }

            return $"({StateFromId} -> {StateToId}) | Parameters: {(Parameters != null ? sb.ToString() : "<null>")}";
        }
    }
}