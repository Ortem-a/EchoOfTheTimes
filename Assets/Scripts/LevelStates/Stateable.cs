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

        //private StateService _stateService;
        //private LevelStateMachine _levelStateMachine;

        //[Inject]
        //private void Construct(StateService stateService, LevelStateMachine levelStateMachine)
        //{
        //    _stateService = stateService;
        //    _levelStateMachine = levelStateMachine;
        //}

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
    }
}