using UnityEngine;
using Zenject;

namespace Systems.Movement
{
    public interface IUnit : ISpawnable
    {
        public IMovableByPath Movable { get; }
        public ITeleportable Teleportable { get; }
        public void Move(Vertex to);

        public class Factory : PlaceholderFactory<GameObject, IUnit>
        {

        }
    }
}