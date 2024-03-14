using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Units;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Commands
{
    public class CommandManager : MonoBehaviour
    {
        //private IUnit _target;
        private Player _target;

        private List<Vector3> _execCommadns;

        //private bool _isRunning = false;

        private void Awake()
        {
            //_target = GetComponent<IUnit>();
            _target = GetComponent<Player>();
        }

        public void UpdateCommands(List<Vector3> commands)
        {
            _execCommadns = commands;

            _target.MoveTo(_execCommadns);

            //_isRunning = true;
        }

        //public void ForceStop(LevelStateMachine.StateMachineCallback callback = null)
        //{
        //    _isRunning = false;
        //}

        //private void Update()
        //{
        //    if (_isRunning)
        //    {
        //        if (!_target.IsBusy)
        //        {
        //            _target.MoveTo(_execCommadns[0]);

        //            _execCommadns.RemoveAt(0);

        //            if (_execCommadns.Count == 0)
        //            {
        //                _isRunning = false;
        //            }
        //        }
        //    }
        //}
    }
}