using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Movement
{
    [Serializable]
    public class Vertex : MonoBehaviour, IEquatable<Vertex>
    {
        public int Id;
        public List<Edge> Neighbours;

        public bool IsBridge = false;
        public bool IsMoving = false;

        public IndicationPlaceholder IndicationPlaceholder { get; private set; }

        protected virtual void Awake()
        {
            IndicationPlaceholder = GetComponentInChildren<IndicationPlaceholder>();
        }

        public bool ContainsNeighbour(Vertex v)
        {
            if (Neighbours != null)
            {
                foreach (Edge e in Neighbours)
                {
                    if (e.Vertex.Id == v.Id)
                        return true;
                }
            }

            return false;
        }

        public bool Equals(Vertex other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }

        public override string ToString()
        {
            return $"[{name}] ID: {Id} | Neighbours count: {Neighbours.Count}";
        }

        public override bool Equals(object other) => other is Vertex vertex && Equals(vertex);

        public override int GetHashCode() => HashCode.Combine(Id, Neighbours);
    }
}