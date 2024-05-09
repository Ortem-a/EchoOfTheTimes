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
            //Stateable = GetComponent<Stateable>();

            //if (Stateable == null)
            //{
            //    Stateable = gameObject.AddComponent<Stateable>();
            //}

            Stateable.States = new System.Collections.Generic.List<StateParameter>();
        }

        public void SetOrUpdateState(int stateId)
        {
            Stateable.CurrentStateId = stateId;

            Stateable.SetOrUpdateParamsToState();
        }
    }
}