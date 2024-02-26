using System;

namespace EchoOfTheTimes.Core
{
    [Serializable]
    public class Edge : IComparable<Edge>
    {
        public float Cost;
        public Vertex Vertex;

        public Edge(Vertex vertex = null, float cost = 1f)
        {
            Vertex = vertex;
            Cost = cost;
        }

        public int CompareTo(Edge other)
        {
            float result = Cost - other.Cost;
            int idA = Vertex.GetInstanceID();
            int idB = other.Vertex.GetInstanceID();

            if (idA == idB) 
            {
                return 0;
            }

            return (int)result;
        }

        public bool Equals(Edge other) 
        {
            return (other.Vertex.Id == Vertex.Id);
        }

        public override bool Equals(object obj)
        {
            Edge other = (Edge)obj;
            return (other.Vertex.Id == Vertex.Id);
        }

        public override int GetHashCode()
        {
            return Vertex.GetHashCode();
        }
    }
}