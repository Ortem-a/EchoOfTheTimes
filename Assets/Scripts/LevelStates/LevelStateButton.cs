using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateButton : MonoBehaviour
    {
        public bool IsEnable = true;
        public bool IsPressed = false;

        public LevelStateMachine StateMachine;

        public List<Transition> Transitions;

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
            StateMachine.SetParamsToTransitions(Transitions);
        }
    }
}