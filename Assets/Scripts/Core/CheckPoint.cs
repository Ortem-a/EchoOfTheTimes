using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class Checkpoint : MonoBehaviour
    {
        [field: SerializeField]
        public Vertex Point { get; private set; }

        public bool IsVisited = false;
    }
}