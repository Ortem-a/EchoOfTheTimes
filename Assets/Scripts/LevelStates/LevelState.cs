using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<StateParameter> StatesParameters;

        public void Accept(List<StateParameter> parameters, bool isDebug = false)
        {
            List<Transform> acceptedTargets = null;

            if (parameters != null && parameters.Count != 0)
            {
                acceptedTargets = new List<Transform>();

                foreach (var param in parameters)
                {
                    acceptedTargets.Add(param.Target);

                    param.AcceptState(param, isDebug);
                }
            }

            if (StatesParameters != null)
            {
                for (int i = 0; i < StatesParameters.Count; i++)
                {
                    if (parameters != null && parameters.Count != 0)
                    {
                        if (!acceptedTargets.Contains(StatesParameters[i].Target))
                            StatesParameters[i].AcceptState(isDebug: isDebug);
                    }
                    else
                    {
                        StatesParameters[i].AcceptState(isDebug: isDebug);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Can't accept Level state [{Id}] without objects!");
            }
        }

        private bool AlreadyAccepted(List<StateParameter> parameters, int stateId)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param.StateId == stateId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}