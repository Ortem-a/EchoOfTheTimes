using System.Collections.Generic;
using Systems.Leveling;
using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.Movement
{
    public class GraphVisibility : Graph
    {
        private StatesInvoker _stateService;

        [Inject]
        private void Construct(LevelSettingsScriptableObject levelSettings, StatesInvoker stateService)
        {
            MaxDistanceToNeighbourVertex = levelSettings.MaxDistanceToNeighbourVertex;

            _stateService = stateService;

            _stateService.OnCompleteChangingState += ResetAndLoad;
        }

        private void Awake()
        {
            ResetAndLoad();
        }

        private void OnDestroy()
        {
            _stateService.OnCompleteChangingState -= ResetAndLoad;
        }

        public List<Vertex> GetVertices()
        {
            return vertices;
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

        public virtual void ResetAndLoad(int stateId = 0)
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
                vertices[i].Id = i;
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