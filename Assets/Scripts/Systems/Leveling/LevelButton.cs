using Systems.Movement;
using Systems.Units;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public sealed class LevelButton : AbstractSpecialVertex
    {
        public StateableByButton[] Stateables;

        [field: SerializeField]
        public bool IsActivated { get; private set; } = false;

        private GraphVisibility _graph;

        [Inject]
        private void Construct(GraphVisibility graph)
        {
            _graph = graph;
        }

        public override void OnEnter(IUnit unit)
        {
            if (Stateables == null) return;

            if (!IsActivated)
            {
                IsActivated = true;

                unit.Movable.StopImmediate();

                for (int i = 0; i < Stateables.Length; i++)
                {
                    Stateables[i].AcceptState(
                        1,
                        () => _graph.ResetAndLoad());
                }
            }
        }

        public override void OnExit(IUnit unit) { }
    }
}