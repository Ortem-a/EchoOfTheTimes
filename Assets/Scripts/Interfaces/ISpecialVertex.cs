using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Interfaces
{
    public interface ISpecialVertex
    {
        public Action OnEnter { get; }
        public Action OnExit { get; }

        public void Initialize();
    }
}