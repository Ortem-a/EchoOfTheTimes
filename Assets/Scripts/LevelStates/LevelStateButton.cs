using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateButton : MonoBehaviour
    {
        public bool IsPressed { get; private set; } = false;

        public delegate void ButtonEventHandler();
        public ButtonEventHandler OnPress;
        public ButtonEventHandler OnRelease;

        public LevelStateMachine StateMachine;

        [Space]
        [Space]
        [InspectorButton(nameof(AcceptInfluenceToObjects))]
        public bool IsAcceptInfluenceToObjects;
        [Space]
        [Space]

        public List<SpecialTransition> Influences;

        private void OnEnable()
        {
            OnPress += Press;
            OnRelease += Release;
        }

        private void OnDisable()
        {
            OnPress -= Press;
            OnRelease -= Release;
        }

        private void Press()
        {
            IsPressed = true;

            Debug.Log($"[LevelStateButton] {name} pressed! IsPressed: {IsPressed}");

            ExecutePress();

            StateMachine.SetParamsToTransitions(Influences);
        }

        private void Release()
        {
            IsPressed = false;

            Debug.Log($"[LevelStateButton] {name} released! IsPressed: {IsPressed}");

            ExecuteRelease();

            StateMachine.RemoveParamsFromTransitions(Influences);
        }

        private void ExecutePress()
        {
            foreach (SpecialTransition specTransition in Influences)
            {
                if (specTransition.EqualsWith(StateMachine.LastTransition))
                {
                    foreach (var stateable in specTransition.Influenced)
                    {
                        var transition = stateable.SpecialTransitions.Find(
                            (x) => x.StateFromId == specTransition.StateFromId && x.StateToId == specTransition.StateToId);

                        stateable.TransitSpecial(transition.StateFromId, transition.StateToId);
                    }
                }
            }
        }

        private void ExecuteRelease()
        {
            foreach (SpecialTransition specTransition in Influences)
            {
                if (specTransition.EqualsWith(StateMachine.LastTransition))
                {
                    foreach (var stateable in specTransition.Influenced)
                    {
                        var transition = stateable.SpecialTransitions.Find(
                            (x) => x.StateFromId == specTransition.StateFromId && x.StateToId == specTransition.StateToId);

                        stateable.Transit(transition.StateFromId);
                    }
                }
            }
        }

        public void AcceptInfluenceToObjects()
        {
            if (Influences != null)
            {
                foreach (SpecialTransition influence in Influences)
                {
                    if (influence.Influenced != null)
                    {
                        foreach (Stateable stateable in influence.Influenced)
                        {
                            Transition trans = new Transition()
                            {
                                StateFromId = influence.StateFromId,
                                StateToId = influence.StateToId,
                            };

                            stateable.SpecialTransitions.Add(trans);
                        }
                    }
                }
            }
        }
    }
}