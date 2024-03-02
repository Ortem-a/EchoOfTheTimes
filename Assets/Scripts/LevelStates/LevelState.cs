using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<ObjectState> Objects;

        public void Accept()
        {
            if (Objects != null)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    Objects[i].AcceptState();
                }
            }
            else
            {
                Debug.LogWarning($"Can't accept Level state [{Id}] without objects!");
            }
        }
    }
}