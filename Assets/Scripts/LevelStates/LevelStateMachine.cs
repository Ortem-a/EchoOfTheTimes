using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateMachine : MonoBehaviour
    {
        [Space]
        [InspectorButton(nameof(InitializeFirstState))]
        public bool _isInitFirstState;
        [Space]

        public List<LevelState> States;
        public List<Transicion> Transicions;

        private LevelState _current;

        private void Awake()
        {
            LoadDefaultState();
        }

        private void LoadDefaultState()
        {
            _current = States[0];
            _current.Accept();
        }

        public void ChangeState(LevelState newState)
        {
            ChangeState(newState.Id);
        }

        public void ChangeState(int newStateId)
        {
            var transicion = Transicions.Find((x) => x.StateFromId == _current.Id && x.StateToId == newStateId);

            if (transicion != null)
            {
                _current = States.Find((x) => x.Id == newStateId);
                _current.Accept();
            }
        }

        public void InitializeFirstState()
        {
            var objects = FindObjectsOfType<StateParameter>();

            var objsState = new List<ObjectState>();

            for (int i = 0; i < objects.Length; i++)
            {
                objsState.Add(new ObjectState()
                {
                    Target = objects[i].Target,
                    Position = objects[i].Position,
                    Rotation = objects[i].Rotation,
                    LocalScale = objects[i].LocalScale,
                });
            }

            LevelState firstState = new LevelState()
            {
                Id = 0,
                Objects = objsState
            };

            States = new List<LevelState> { firstState };
        }
    }
}