using UnityEngine;

namespace EchoOfTheTimes.Core
{
    [System.Serializable]
    public class SubGraphBridge
    {
        public Vertex Side1;
        public Vertex Side2;

        public void Connect(float maxDistanceToNeighbour)
        {
            AddToNeighbours(Side1, Side2, maxDistanceToNeighbour);
            AddToNeighbours(Side2, Side1, maxDistanceToNeighbour);
        }

        public void Disconnect()
        {
            RemoveFromNeighbours(Side1, Side2);
            RemoveFromNeighbours(Side2, Side1);
        }

        private void AddToNeighbours(Vertex newNeighbour, Vertex to, float maxDistanceToNeighbour)
        {
            var dist = Vector3.Distance(newNeighbour.transform.position, to.transform.position);

            if (dist > maxDistanceToNeighbour) return;

            to.Neighbours.Add(new Edge(newNeighbour, dist));
        }

        private void RemoveFromNeighbours(Vertex toRemove, Vertex from)
        {
            var edge = from.Neighbours.Find(x => x.Vertex.Id == toRemove.Id);
            if (edge != null)
            {
                from.Neighbours.Remove(edge);
            }
        }
    }
}