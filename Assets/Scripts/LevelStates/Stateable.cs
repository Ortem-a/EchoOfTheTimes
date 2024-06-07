using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(StateableGizmosDrawer))]
    public class Stateable : MonoBehaviour
    {
        public int CurrentStateId;

        public bool IsLocalSpace;

        public List<StateParameter> States = new List<StateParameter>();

#if UNITY_EDITOR
        public void SetOrUpdateParamsToState()
        {
            var stateParam = States.Find((x) => x.StateId == CurrentStateId);

            Vector3 newPosition = transform.position;
            Vector3 newRotation = transform.rotation.eulerAngles;
            if (IsLocalSpace)
            {
                newPosition = transform.localPosition;
                newRotation = transform.localRotation.eulerAngles;
            }

            var newStateParam = new StateParameter
            {
                StateId = CurrentStateId,
                Target = transform,
                Position = newPosition,
                Rotation = newRotation,
                LocalScale = transform.localScale,
                IsLocalSpace = IsLocalSpace
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
                if (IsLocalSpace)
                {
                    transform.SetLocalPositionAndRotation(stateParam.Position, Quaternion.Euler(stateParam.Rotation));
                }
                else
                {
                    transform.SetPositionAndRotation(stateParam.Position, Quaternion.Euler(stateParam.Rotation));
                }
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