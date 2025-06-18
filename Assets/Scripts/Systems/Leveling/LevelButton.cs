using Systems.Movement;
using Systems.Units;
using Zenject;

namespace Systems.Leveling
{
    public sealed class LevelButton : AbstractSpecialVertex
    {
        public StateableByButton[] Stateables;

        private bool _isActivated = false;

        private GraphVisibility _graph;

        [Inject]
        private void Construct(GraphVisibility graph)
        {
            _graph = graph;
        }

        public override void OnEnter(IUnit unit)
        {
            if (Stateables == null) return;

            if (!_isActivated)
            {
                _isActivated = true;

                unit.Movable.StopImmediate();

                for (int i = 0; i < Stateables.Length; i++)
                {
                    Stateables[i].AcceptState(1, _graph.ResetAndLoad);
                }
            }
        }

        public override void OnExit(IUnit unit) { }
    }
}