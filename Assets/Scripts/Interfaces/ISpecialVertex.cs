using System;

namespace EchoOfTheTimes.Interfaces
{
    public interface ISpecialVertex
    {
        public Action OnEnter { get; }
        public Action OnExit { get; }

        public void Initialize();
    }
}