using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class VertexVisibility : Vertex
    {
        //private void Awake()
        //{
        //    Neighbours = new List<Edge>();
        //}

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.25f);

            foreach (var n in Neighbours)
            {
                GizmosHelper.DrawArrowBetween(transform.position, n.Vertex.transform.position, Color.yellow);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, n.Vertex.transform.position);
            }
        }

        public void FindAllNeighbours(List<Vertex> vertices)
        {
            Collider collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;

            Vector3 direction = Vector3.zero;
            Vector3 origin = transform.position;
            Vector3 target = Vector3.zero;
            RaycastHit[] hits;
            Ray ray;
            float distance = 0f;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] == this) continue;

                target = vertices[i].transform.position;
                direction = target - origin;
                distance = direction.magnitude;
                ray = new Ray(origin, direction);
                hits = Physics.RaycastAll(ray, distance);

                if (hits.Length == 1)
                {
                    if (hits[0].collider.gameObject.CompareTag("Vertex"))
                    {
                        Edge edge = new Edge();
                        edge.Cost = distance;
                        GameObject go = hits[0].collider.gameObject;
                        Vertex vertex = go.GetComponent<Vertex>();
                        if (vertex != vertices[i])
                        {
                            continue;
                        }

                        edge.Vertex = vertex;
                        Neighbours.Add(edge);
                    }
                }
            }

            collider.enabled = true;
        }

        public void FindNeighboursInRadius(List<Vertex> vertices, float radius)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] == this)
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