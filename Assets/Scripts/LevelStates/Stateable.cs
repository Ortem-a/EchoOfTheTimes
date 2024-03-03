using EchoOfTheTimes.EditorTools;
using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(StateableGizmosDrawer))]
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

        [Space]
        public int CurrentSpecialStateFromId;
        public int CurrentSpecialStateToId;
        public int CurrentSpecialStateId;
        [Space]
        [InspectorButton(nameof(SetOrUpdateParamsToSpecialState))]
        public bool IsSetParamsToSpecialState;
        [Space]
        [Space]
        [InspectorButton(nameof(TransformObjectBySpecialState))]
        public bool IsTransformObjectBySpecialState;
        [Space]

        public List<Transition> SpecialTransitions = new List<Transition>();

        public void SetOrUpdateParamsToState()
        {
            var stateParam = States.Find((x) => x.StateId == CurrentStateId);

            var newStateParam = new StateParameter
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

        public void SetOrUpdateParamsToSpecialState()
        {
            var specTransition = SpecialTransitions.Find(
                (x) => x.StateFromId == CurrentSpecialStateFromId && x.StateToId == CurrentSpecialStateToId);

            var newSpecTransition = new StateParameter
            {
                StateId = CurrentSpecialStateId,
                Target = transform,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                LocalScale = transform.localScale,
            };

            if (specTransition != null)
            {
                int index = SpecialTransitions.FindIndex(
                    (x) => x.StateFromId == CurrentSpecialStateFromId && x.StateToId == CurrentSpecialStateToId);
                if (SpecialTransitions[index].Parameters == null)
                {
                    SpecialTransitions[index].Parameters = new List<StateParameter> { newSpecTransition };
                }
                else if (SpecialTransitions[index].Parameters.Count == 0)
                {
                    SpecialTransitions[index].Parameters.Add(newSpecTransition);
                }
                else
                {
                    SpecialTransitions[index].Parameters[0] = newSpecTransition;
                }
            }
            else
            {
                Debug.LogWarning($"There is no special transition with params: " +
                    $"{CurrentSpecialStateFromId} -> {CurrentSpecialStateToId} | " +
                    $"Current special state Id: {CurrentSpecialStateId}");
                Debug.LogWarning("Add special transition with some button first!");
            }
        }

        public void TransformObjectBySpecialState()
        {
            var specTransition = SpecialTransitions.Find((x) =>
                x.StateFromId == CurrentSpecialStateFromId && x.StateToId == CurrentSpecialStateToId);

            if (specTransition != null)
            {
                transform.SetPositionAndRotation(
                    specTransition.Parameters[0].Position,
                    Quaternion.Euler(specTransition.Parameters[0].Rotation));
                transform.localScale = specTransition.Parameters[0].LocalScale;
            }
            else
            {
                Debug.LogWarning($"There is no special transition with [{CurrentSpecialStateFromId} -> {CurrentSpecialStateToId}]!");
            }
        }
    }
}