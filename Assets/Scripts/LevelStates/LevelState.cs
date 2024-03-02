using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<StateParameter> StatesParameters;

        public void Accept()
        {
            if (StatesParameters != null)
            {
                for (int i = 0; i < StatesParameters.Count; i++)
                {
                    StatesParameters[i].AcceptState();
                }
            }
            else
            {
                Debug.LogWarning($"Can't accept Level state [{Id}] without objects!");
            }
        }
    }
}