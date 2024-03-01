using EchoOfTheTimes.Animations;
using EchoOfTheTimes.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace EchoOfTheTimes.Units
{
    [RequireComponent(typeof(AnimationManager))]
    public class Player : MonoBehaviour, IUnit
    {
        // add animation manager here
        private AnimationManager _animationManager;
        public AnimationManager Animations => 
            _animationManager = _animationManager != null ? _animationManager : GetComponent<AnimationManager>();

        [field: SerializeField]
        public float Speed { get; set; } = 5f;

        public virtual void TeleportTo(Vector3 position)
        {
            Debug.Log($"[TeleportTo] {position} with speed: {Speed}");

            transform.position = position;
        }

        public virtual void MoveTo(Vector3 destination)
        {
            if (Vector3.Distance(transform.position, destination) < Mathf.Epsilon)
            {
                Debug.Log("COMPLETE");
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
            }
        }
    }
}