using EchoOfTheTimes.Animations;
using System.Threading.Tasks;
using UnityEngine;

namespace EchoOfTheTimes.Interfaces
{
    public interface IUnit
    {
        public AnimationManager Animations { get; }

        public float Speed { get; set; }

        public void TeleportTo(Vector3 position);

        public void MoveTo(Vector3 destinaton);
    }
}