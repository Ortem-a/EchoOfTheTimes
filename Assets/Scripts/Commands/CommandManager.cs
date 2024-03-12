using EchoOfTheTimes.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Commands
{
    public class CommandManager : MonoBehaviour
    {
        private IUnit _target;

        private List<Vector3> _execCommadns;

        private bool _isRunning = false;

        private void Awake()
        {
            _target = GetComponent<IUnit>();
        }

        public void UpdateCommands(List<Vector3> commands)
        {
            _execCommadns = commands;

            _isRunning = true;
        }

        public void ForceStop()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if (_isRunning)
            {
                if (!_target.IsBusy)
                {
                    _target.MoveTo(_execCommadns[0]);

                    _execCommadns.RemoveAt(0);

                    if (_execCommadns.Count == 0)
                    {
                        _isRunning = false;
                    }
                }
            }
        }
    }
}