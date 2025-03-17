using UnityEditor;
using UnityEngine;
using static Google.DialogWindow;

namespace Systems.Leveling
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

#if UNITY_EDITOR
        public void Initialize()
        {
            Stateable.States = new Tools.StateableSerializableDictionary();
        }

        public void SetOrUpdateState(int stateId)
        {
            Stateable.States.AddOrUpdate(stateId, transform);
        }

        public void TransformStairsToState(int stateId)
        {
            if (Stateable.States.TryGetValue(stateId, out var stateOption))
            {
                stateOption.Target.SetLocalPositionAndRotation(
                    stateOption.LocalPosition, stateOption.LocalRotation);
                stateOption.Target.localScale = stateOption.LocalScale;
            }
            else
            {
                Debug.LogWarning($"[STAIR - {name}] There is no state with id: {stateId}!");
            }
        }
#endif
    }
}