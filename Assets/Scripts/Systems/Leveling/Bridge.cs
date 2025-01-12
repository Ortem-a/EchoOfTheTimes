using System;
using Systems.Movement;

namespace Systems.Leveling
{
    [Serializable]
    public class Bridge
    {
        public Vertex Inner;
        public Vertex Outer;

        public void Connect()
        {
            Inner.Neighbours.Add(new Edge(Outer));
            Outer.Neighbours.Add(new Edge(Inner));
        }

        public void Disconnect()
        {
            for (int i = 0; i < Inner.Neighbours.Count; i++)
            {
                if (Inner.Neighbours[i].Vertex == Outer)
                {
                    Inner.Neighbours.RemoveAt(i);
                }
            }

            for (int i = 0; i < Outer.Neighbours.Count; i++)
            {
                if (Outer.Neighbours[i].Vertex == Inner)
                {
                    Outer.Neighbours.RemoveAt(i);
                }
            }
        }
    }
}