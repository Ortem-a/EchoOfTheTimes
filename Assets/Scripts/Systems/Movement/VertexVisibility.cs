using System.Collections.Generic;
using UnityEngine;

namespace Systems.Movement
{
    public class VertexVisibility : Vertex
    {
        public void FindNeighboursInRadius(List<Vertex> vertices, float radius)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] == this || ContainsNeighbour(vertices[i]))
                    continue;

                var dist = Vector3.Distance(transform.position, vertices[i].transform.position);
                if (dist <= radius)
                {
                    Vertex vertex = vertices[i].gameObject.GetComponent<Vertex>();

                    Edge edge = new Edge(vertex, dist);

                    Neighbours.Add(edge);

                    vertex.Neighbours.Add(new Edge(this, dist));
                }
            }
        }
    }
}