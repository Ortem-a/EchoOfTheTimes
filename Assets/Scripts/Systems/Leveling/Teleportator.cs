using System;
using Systems.Movement;
using UnityEngine;

namespace Systems.Leveling
{
    public class Teleportator : MonoBehaviour, ISpecialVertex
    {
        [field: SerializeField]
        public Teleportator Destination { get; set; }

        public Action OnStartEnter { get; private set; } = null;

        public Action OnCompleteEnter { get; private set; } = null;

        public Action OnStartExit { get; private set; } = null;

        public Action OnCompleteExit { get; private set; } = null;

        public Vertex Vertex => GetComponentInParent<Vertex>();

        public void OnEnter()
        {
            OnStartEnter?.Invoke();

            Teleportate();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        private void Teleportate()
        {
            //OnCompleteEnter?.Invoke(); !!!!!!!

            throw new System.NotImplementedException();
        }
    }
}