using Systems.Core;
using Systems.Units;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public class FinishButton : MonoBehaviour, ISpecialVertex
    {
        public SpecialVertexType Type => SpecialVertexType.Button;

        private GameLoopService _gameLoop;

        [Inject]
        private void Construct(GameLoopService gameLoop)
        {
            _gameLoop = gameLoop;
        }

        public void OnEnter(IUnit unit)
        {
            Debug.Log($"[FINISH BUTTON] {unit} enter");

            _gameLoop.FinishLevel();
        }

        public void OnExit(IUnit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}