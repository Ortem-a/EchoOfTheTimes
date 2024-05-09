using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class Stair : MonoBehaviour
    {
        private Stateable _stateable;

        public void Initialize()
        {
            if (!TryGetComponent(out _stateable))
            {
                _stateable = gameObject.AddComponent<Stateable>();
            }

            _stateable.States = new System.Collections.Generic.List<StateParameter>();
        }

        public void SetOrUpdateState(int stateId)
        {
            _stateable.CurrentStateId = stateId;

            _stateable.SetOrUpdateParamsToState();
        }
    }
}