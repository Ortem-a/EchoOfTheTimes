using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class CheckPoint : MonoBehaviour
    {
        public Vector3 Point { get; private set; }

        public void SetCheckPoint()
        {
            if (transform.parent.TryGetComponent(out Vertex vertex))
            {
                Point = vertex.transform.position;
            }
            else
            {
                Debug.LogError($"Incorrect place for checkpoint '{name}' as child of '{transform.parent}'! " +
                    $"You have to place it as a child of Vertex!");
            }
        }
    }
}