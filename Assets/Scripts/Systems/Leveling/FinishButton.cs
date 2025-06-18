using Systems.Core;
using Systems.Units;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public sealed class FinishButton : AbstractSpecialVertex
    {
        private GameLoopService _gameLoop;

        [Inject]
        private void Construct(GameLoopService gameLoop)
        {
            _gameLoop = gameLoop;
        }

        public override void OnEnter(IUnit unit)
        {
            Debug.Log($"[FINISH BUTTON] {unit} enter");

            _gameLoop.FinishLevel();
        }

        public override void OnExit(IUnit unit) { }
    }
}