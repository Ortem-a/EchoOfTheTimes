namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class Transition
    {
        public int StateFromId;
        public int StateToId;

        public override string ToString()
        {
            return $"({StateFromId} -> {StateToId})";
        }
    }
}