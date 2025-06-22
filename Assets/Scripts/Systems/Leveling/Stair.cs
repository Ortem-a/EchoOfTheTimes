namespace Systems.Leveling
{
    public class Stair : Stateable 
    {
        public void SetOrUpdateOption(int stateId)
        {
            SetOptionsForState(stateId, transform);
        }
    }
}