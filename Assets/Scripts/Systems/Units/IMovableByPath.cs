using System;
using System.Collections.Generic;
using Systems.Movement;
using UnityEngine;

namespace Systems.Units
{
    public interface IMovableByPath
    {
        public Queue<Vertex> Path { get; }
        public Vertex CurrentWaypoint { get; set; }
        public Vertex NextWaypoint { get; }
        public Vector3 Direction { get; }
        public Action<Vertex> OnWaypointChanged { get; set; }
        public Action OnEnterToBridge { get; }
        public bool NeedStop { get; }
        public bool IsMoving { get; }
        public bool OnBridge { get; }
        public float Speed { get; }
        public MarkerParent TempParent { get; }
        public void MoveBy(List<Vertex> path);
        public void Stop(Action onStopped = null);
        public void StopImmediate();
        public void SetParent(Vertex vertex);
    }
}