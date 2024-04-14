using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(StateableGizmosDrawer))]
    public class Stateable : MonoBehaviour
    {
        public int CurrentStateId;

        public List<StateParameter> States = new List<StateParameter>();

        public int CurrentSpecialStateFromId;
        public int CurrentSpecialStateToId;
        public int CurrentSpecialStateId;

        public List<Transition> SpecialTransitions = new List<Transition>();

        private StateService _stateService;

        [Inject]
        private void Construct(StateService stateService)
        {
            _stateService = stateService;
        }

        public void TransitSpecial(int fromId, int toId)
        {
            var spec = SpecialTransitions.Find((x) => x.StateFromId == fromId && x.StateToId == toId);

            if (spec != null)
            {
                int index = SpecialTransitions.FindIndex((x) => x.StateFromId == fromId && x.StateToId == toId);

                foreach (StateParameter parameters in SpecialTransitions[index].Parameters)
                {
                    //parameters.AcceptState(null, parameters);
                    _stateService.AcceptState(null, parameters);
                }
            }
        }

        public void Transit(int toId)
        {
            var state = States.Find((x) => x.StateId == toId);

            if (state != null)
            {
                int index = States.FindIndex((x) => x.StateId == toId);

                //States[index].AcceptState();
                _stateService.AcceptState(States[index]);
            }
        }

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