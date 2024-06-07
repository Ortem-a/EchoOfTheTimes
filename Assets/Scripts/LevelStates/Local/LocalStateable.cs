using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates.Local
{
    [RequireComponent(typeof(StateableGizmosDrawer))]
    public class LocalStateable : MonoBehaviour, IStateable
    {
        [field: SerializeField]
        public int CurrentStateId { get; set; }

        [field: SerializeField]
        public List<IStateParameter> States { get; set; } = new List<IStateParameter>();

#if UNITY_EDITOR
        public void SetOrUpdateParamsToState()
        {
            var stateParam = States.Find((x) => x.StateId == CurrentStateId);

            var newStateParam = new LocalStateParameter
            {
                StateId = CurrentStateId,
                Target = transform,
                Position = transform.localPosition,
                Rotation = transform.localRotation.eulerAngles,
                LocalScale = transform.localScale,
            };

            if (stateParam != null)
            {
                int index = States.FindIndex((x) => x.StateId == CurrentStateId);
                States[index] = newStateParam;
            }
            else
            {
                States.Add(newStateParam);
            }
        }

        public void TransformObjectByState()
        {
            var stateParam = States.Find((x) => x.StateId == CurrentStateId);

            if (stateParam != null)
            {
                transform.SetLocalPositionAndRotation(stateParam.Position, Quaternion.Euler(stateParam.Rotation));
                transform.localScale = stateParam.LocalScale;
            }
            else
            {
                Debug.LogWarning($"There is no state with Id [{CurrentStateId}]!");
            }
        }
#endif
    }
}