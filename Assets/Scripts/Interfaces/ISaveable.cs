using EchoOfTheTimes.Utils;

namespace EchoOfTheTimes.Interfaces
{
    public interface ISaveable
    {
        SerializableGuid Id { get; set; }
    }
}