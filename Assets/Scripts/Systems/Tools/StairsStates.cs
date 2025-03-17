namespace Systems.Tools
{
    public enum StairStateType
    {
        FlatBottom,
        FlatTop,
        StartBottom,
        StartTop
    }

    [System.Serializable]
    public class StairsStates
    {
        public StairStateType StairState;
        public int[] StateIds;
    }
}