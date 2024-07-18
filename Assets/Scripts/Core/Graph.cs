using EchoOfTheTimes.ScriptableObjects.Level;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    public abstract class Graph : MonoBehaviour
    {
        protected float MaxDistanceToNeighbourVertex;

        protected List<Vertex> vertices;
        protected List<List<Vertex>> neighbours;
        protected List<List<float>> costs;

        [Inject]
        private void Construct(LevelSettingsScriptableObject levelSettings)
        {
            MaxDistanceToNeighbourVertex = levelSettings.MaxDistanceToNeighbourVertex;
        }

        public virtual void Awake()
        {
            Load();
        }

        public virtual void Load() { }

        public virtual int GetSize()
        {
            if (vertices is null)
            {
                return 0;
            }

            return vertices.Count;
        }

        public virtual Vertex GetNearestVertex(Vector3 position)
        {
            return null;
        }

        public virtual Vertex GetVertexObj(int id)
        {
            if (vertices is null || vertices.Count == 0)
            {
                return null;
            }
            if (id < 0 || id >= vertices.Count)
            {
                return null;
            }

            return vertices[id];
        }

        public virtual Vertex[] GetNeighbours(Vertex vertex)
        {
            if (neighbours is null || neighbours.Count == 0)
            {
                return new Vertex[0];
            }

            if (vertex.Id < 0 || vertex.Id >= neighbours.Count)
            {
                return new Vertex[0];
            }

            return neighbours[vertex.Id].ToArray();
        }

        public virtual Edge[] GetEdges(Vertex vertex)
        {
            return null;
        }

        public List<Vertex> GetPathBFS(GameObject srcObj, GameObject dstObj)
        {
            if (srcObj == null || dstObj == null)
            {
                return new List<Vertex>();
            }

            Vertex[] neighbours;
            Queue<Vertex> queue = new Queue<Vertex>();
            Vertex source = GetNearestVertex(srcObj.transform.position);
            Vertex destination = GetNearestVertex(dstObj.transform.position);
            Vertex vertex;
            int[] previous = new int[vertices.Count];

            for (int i = 0; i < previous.Length; i++)
            {
                previous[i] = -1;
            }
            previous[source.Id] = source.Id;
            queue.Enqueue(source);

            while (queue.Count != 0)
            {
                vertex = queue.Dequeue();

                if (ReferenceEquals(vertex, destination))
                {
                    return BuildPath(source.Id, vertex.Id, ref previous);
                }

                neighbours = GetNeighbours(vertex);

                foreach (Vertex neighbour in neighbours)
                {
                    if (previous[neighbour.Id] != -1)
                    {
                        continue;
                    }
                    previous[neighbour.Id] = vertex.Id;
                    queue.Enqueue(neighbour);
                }
            }

            return new List<Vertex>();
        }

        public List<Vertex> GetPathBFS(Vertex src, Vertex dst)
        {
            if (src == null || dst == null)
            {
                return new List<Vertex>();
            }

            Vertex[] neighbours;
            Queue<Vertex> queue = new Queue<Vertex>();
            Vertex source = GetNearestVertex(src.transform.position);
            Vertex destination = GetNearestVertex(dst.transform.position);
            Vertex vertex;
            int[] previous = new int[vertices.Count];

            for (int i = 0; i < previous.Length; i++)
            {
                previous[i] = -1;
            }
            previous[source.Id] = source.Id;
            queue.Enqueue(source);

            while (queue.Count != 0)
            {
                vertex = queue.Dequeue();

                if (ReferenceEquals(vertex, destination))
                {
                    return BuildPath(source.Id, vertex.Id, ref previous);
                }

                neighbours = GetNeighbours(vertex);

                foreach (Vertex neighbour in neighbours)
                {
                    if (previous[neighbour.Id] != -1)
                    {
                        continue;
                    }
                    previous[neighbour.Id] = vertex.Id;
                    queue.Enqueue(neighbour);
                }
            }

            return new List<Vertex>();
        }

        private List<Vertex> BuildPath(int sourceId, int destinationId, ref int[] prevList)
        {
            List<Vertex> path = new List<Vertex>();
            int prev = destinationId;

            do
            {
                path.Add(vertices[prev]);
                prev = prevList[prev];
            }
            while (prev != sourceId);

            return path;
        }

        public bool HasPath(Vertex from, Vertex to) => GetPathBFS(from, to).Count != 0;
    }
}