using EchoOfTheTimes.Collectables;

namespace EchoOfTheTimes.Interfaces
{
    public interface ICollectable
    {
        public int Id { get; }
        public CollectableStatusType Status { get; }
        public void Collect();
    }

    public interface ISpawnable
    {
        public void Spawn(int id, CollectableStatusType status);
    }
}