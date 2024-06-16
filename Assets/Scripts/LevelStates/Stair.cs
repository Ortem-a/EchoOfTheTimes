using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class Stair : MonoBehaviour
    {
        private Stateable _stateable;
        public Stateable Stateable 
        {
            get
            {
                _stateable = _stateable != null ? _stateable : GetComponent<Stateable>();

                if (_stateable == null)
                {
                    _stateable = gameObject.AddComponent<Stateable>();
                }

                return _stateable;
            }
            private set => _stateable = value;
        }

        public void Initialize()
        {
            Stateable.States = new System.Collections.Generic.List<StateParameter>();
        }

#if UNITY_EDITOR
        public void SetOrUpdateState(int stateId)
        {
            Stateable.CurrentStateId = stateId;

            Stateable.SetOrUpdateParamsToState();
        }

        public void TransformStairsToState(int stateId)
        {
            Stateable.CurrentStateId = stateId;

            Stateable.TransformObjectByState();
        }
#endif
    }
}