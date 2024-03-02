using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class Stateable : MonoBehaviour
    {
        [Space]
        public int CurrentStateId;
        [Space]
        [InspectorButton(nameof(SetOrUpdateParamsToState))]
        public bool IsSetParamsToState;
        [Space]
        [Space]
        [InspectorButton(nameof(TransformObjectByState))]
        public bool IsTransformObjectByState;
        [Space]

        public List<StateParameter> States = new List<StateParameter>();

        private void OnDrawGizmosSelected()
        {
            if (States != null) 
            {
                foreach (var state in States) 
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(state.Position, state.LocalScale);
                }
            }
        }

        public void AcceptState(int stateId)
        {
            var state = States.Find((x) => x.StateId == stateId);

            if (state != null) 
            {
                state.AcceptState();
            }
            else
            {
                Debug.LogWarning($"[{name}] There is no state with Id [{stateId}]");
            }
        }

        public void SetOrUpdateParamsToState()
        {
            var stateParam = States.Find((x) => x.StateId == CurrentStateId);

            var newStateParam  =new StateParameter
            {
                StateId = CurrentStateId,
                Target = transform,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
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
                transform.SetPositionAndRotation(stateParam.Position, Quaternion.Euler(stateParam.Rotation));
                transform.localScale = stateParam.LocalScale;
            }
            else
            {
                Debug.LogWarning($"There is no state with Id [{CurrentStateId}]!");
            }
        }
    }
}