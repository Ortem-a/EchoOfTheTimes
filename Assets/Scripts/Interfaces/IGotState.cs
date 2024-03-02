using UnityEngine;

namespace EchoOfTheTimes.Interfaces
{
    public interface IGotState
    {
        Transform Target { get; }
        Vector3 Position { get; }
        Vector3 Rotation { get; }
        Vector3 LocalScale { get; }
    }
}