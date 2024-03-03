using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateButton : MonoBehaviour
    {
        public bool IsEnable = true;
        public bool IsPressed = false;

        public LevelStateMachine StateMachine;

        [Space]
        [Space]
        [InspectorButton(nameof(AcceptInfluenceToObjects))]
        public bool IsAcceptInfluenceToObjects;
        [Space]
        [Space]

        public List<SpecialTransition> Influences;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsPressed = !IsPressed;

                Debug.Log($"[LevelStateButton] {name} pressed! IsPressed: {IsPressed}");

                if (IsPressed)
                {
                    OnPressed();
                }
            }
        }

        public void OnPressed()
        {
            if (IsEnable)
            {
                StateMachine.SetParamsToTransitions(Influences);
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