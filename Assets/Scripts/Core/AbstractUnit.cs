using EchoOfTheTimes.Interfaces;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class AbstractUnit : MonoBehaviour, IMovable
    {
        [field: SerializeField]
        public float Speed { get; set; }

        public virtual void MoveTo(Vector3 position)
        {
            Debug.Log($"Move to {position} with speed: {Speed}");

            transform.position = position;// * Speed * Time.deltaTime;
        }
    }
}