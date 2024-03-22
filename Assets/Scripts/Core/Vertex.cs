using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    [System.Serializable]
    public class Vertex : MonoBehaviour
    {
        public int Id;
        public List<Edge> Neighbours;

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
    }
}