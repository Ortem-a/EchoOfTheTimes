using System;
using Systems.Movement;
using UnityEngine;

namespace Systems.Leveling
{
    [Serializable]
    public class Bridge : IEquatable<Bridge>
    {
        public Vertex Inner;
        public Vertex Outer;

        public bool IsConnected { get; private set; }

        public void Connect()
        {
            if (Vector3.Distance(Inner.transform.position, Outer.transform.position) > 2f)
            {
                return;
            }

            Inner.Neighbours.Add(new Edge(Outer));
            Outer.Neighbours.Add(new Edge(Inner));

            IsConnected = true;
        }

        public void Disconnect()
        {
            for (int i = 0; i < Inner.Neighbours.Count; i++)
            {
                if (Inner.Neighbours[i].Vertex == Outer)
                {
                    Inner.Neighbours.RemoveAt(i);
                }
            }

            for (int i = 0; i < Outer.Neighbours.Count; i++)
            {
                if (Outer.Neighbours[i].Vertex == Inner)
                {
                    Outer.Neighbours.RemoveAt(i);
                }
            }

            IsConnected = false;
        }

        public bool Equals(Bridge other)
        {
            if (other == null)
            {
                return false;
            }

            return Inner.Id == other.Outer.Id ||
                Outer.Id == other.Inner.Id ||
                Inner.Id == other.Inner.Id && Outer.Id == other.Outer.Id;
        }
    }
}