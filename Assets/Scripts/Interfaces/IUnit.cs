using EchoOfTheTimes.Animations;
using EchoOfTheTimes.Core;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Interfaces
{
    public interface IUnit
    {
        public AnimationManager Animations { get; }
        public float Speed { get; set; }
        public bool IsBusy { get; set; }
        public Vertex Position { get; }

        public void TeleportTo(Vector3 position);
        public void MoveTo(List<Vector3> waypoints);
    }
}