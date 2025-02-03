using Systems.Movement;
using Systems.Units;
using UnityEngine;

namespace Systems.Leveling
{
    public class LevelButton : MonoBehaviour, ISpecialVertex
    {
        public SpecialVertexType Type => SpecialVertexType.Button;

        public StateableByButton[] Stateables;

        private bool _isActivated = false;

        public void OnEnter(IUnit unit)
        {
            if (Stateables == null) return;

            if (!_isActivated)
            {
                _isActivated = true;

                unit.Movable.StopImmediate();

                for (int i = 0; i < Stateables.Length; i++)
                {
                    Stateables[i].AcceptState(1, null);
                }
            }
        }

        public void OnExit(IUnit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}