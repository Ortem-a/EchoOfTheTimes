using EchoOfTheTimes.LevelStates;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class GraphVisibility : Graph
    {
        public override void Awake()
        {
            ResetAndLoad();
        }

        public override void Load()
        {
            Vertex[] verts = GetComponentsInChildren<Vertex>();
            vertices = new List<Vertex>(verts);

            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Id = i;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                ((VertexVisibility)vertices[i]).FindNeighboursInRadius(vertices, MaxDistanceToNeighbourVertex);
            }
        }

        public void ResetAndLoad()
        {
            Vertex[] verts = GetComponentsInChildren<Vertex>();
            vertices = new List<Vertex>(verts);

            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Id = i;
                vertices[i].Neighbours = new List<Edge>();
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                ((VertexVisibility)vertices[i]).FindNeighboursInRadius(vertices, MaxDistanceToNeighbourVertex);
            }
        }

        public void ResetVertices()
        {
            Vertex[] verts = GetComponentsInChildren<Vertex>();
            vertices = new List<Vertex>(verts);

            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Id = 0;
                vertices[i].Neighbours = new List<Edge>();
            }
        }

        public override Vertex GetNearestVertex(Vector3 position)
        {
            Vertex vertex = null;
            float distance = Mathf.Infinity;
            float distanceNear = distance;
            Vector3 positionVertex;

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

        public Vertex GetNearestVertexInRadius(Vector3 position, float radius)
        {
            Vertex vertex = null;
            float distance = Mathf.Infinity;
            float distanceNear = distance;
            Vector3 positionVertex;

            for (int i = 0; i < vertices.Count; i++)
            {
                positionVertex = vertices[i].transform.position;

                if (Vector3.Distance(position, positionVertex) > radius) continue;

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