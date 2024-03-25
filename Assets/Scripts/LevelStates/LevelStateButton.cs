using EchoOfTheTimes.Core;
using EchoOfTheTimes.Editor;
using EchoOfTheTimes.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateButton : MonoBehaviour, ISpecialVertex
    {
        public bool IsPressed { get; private set; } = false;

        public Action OnEnter => Press;
        public Action OnExit => Release;

        private LevelStateMachine _stateMachine;

        [Space]
        [Space]
        [InspectorButton(nameof(AcceptInfluenceToObjects))]
        public bool IsAcceptInfluenceToObjects;
        [Space]
        [Space]

        public List<SpecialTransition> Influences;

        public void Initialize()
        {
            _stateMachine = GameManager.Instance.StateMachine;
        }

        private void Press()
        {
            IsPressed = true;

            Debug.Log($"[LevelStateButton] {name} pressed! IsPressed: {IsPressed}");

            ExecutePress();

            _stateMachine.SetParamsToTransitions(Influences);
        }

        private void Release()
        {
            IsPressed = false;

            Debug.Log($"[LevelStateButton] {name} released! IsPressed: {IsPressed}");

            ExecuteRelease();

            _stateMachine.RemoveParamsFromTransitions(Influences);
        }

        private void ExecutePress()
        {
            foreach (SpecialTransition specTransition in Influences)
            {
                if (specTransition.EqualsWith(_stateMachine.LastTransition))
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
                if (specTransition.EqualsWith(_stateMachine.LastTransition))
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