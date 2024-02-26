using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class GraphVisibility : Graph
    {
        public override void Load()
        {
            Vertex[] verts = FindObjectsOfType<Vertex>();
            vertices = new List<Vertex>(verts);

            for (int i = 0; i < vertices.Count; i++) 
            {
                VertexVisibility vertexVisibility = vertices[i] as VertexVisibility;
                vertexVisibility.Id = i;
                //vertexVisibility.FindAllNeighbours(vertices);
                vertexVisibility.FindClosestNeighbours(vertices);
            }
        }

        public override Vertex GetNearestVertex(Vector3 position)
        {
            Vertex vertex = null;
            float distance = Mathf.Infinity;
            float distanceNear = distance;
            Vector3 positionVertex = Vector3.zero;

            for (int i = 0; i < vertices.Count; i++) 
            {
                positionVertex = vertices[i].transform.position;
                distance = Vector3.Distance(position, positionVertex);

                if (distance < distanceNear) 
                {
                    distanceNear = distance;
                    vertex = vertices[i];
                }
            }

            return vertex;
        }

        public override Vertex[] GetNeighbours(Vertex vertex)
        {
            List<Edge> edges = vertex.Neighbours;
            Vertex[] ns = new Vertex[edges.Count];

            int i;
            for (i = 0; i < edges.Count; i++) 
            {
                ns[i] = edges[i].Vertex;
            }

            return ns;
        }

        public override Edge[] GetEdges(Vertex vertex)
        {
            return vertices[vertex.Id].Neighbours.ToArray();
        }
    }
}