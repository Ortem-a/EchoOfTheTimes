using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Interfaces
{
    public interface IMovable
    {
        public float Speed { get; set; }

        public void MoveTo(Vector3 position);
    }
}