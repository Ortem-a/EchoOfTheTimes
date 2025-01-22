using System;

namespace Systems.Leveling
{
    public interface ISpecialVertex
    {
        public Action OnStartEnter { get; }
        public Action OnCompleteEnter { get; }
        public Action OnStartExit{ get; }
        public Action OnCompleteExit { get; }

        public void OnEnter();
        public void OnExit();
    }
}