using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Systems.Core
{
    public class LevelLoopManager : IInitializable, IDisposable
    {
        private readonly GameLoopService _gameLoop;

        public LevelLoopManager(GameLoopService gameLoop)
        {
            _gameLoop = gameLoop;
        }

        void IInitializable.Initialize()
        {
            StartLevel();
        }

        void IDisposable.Dispose() 
        {
            FinishLevel();
        }

        private void StartLevel()
        {
            // level loaded
            // prepare UI 
            // show start splash screen
            // when splash screen animation finished -> OnLevelStarted.Invoke()

            throw new NotImplementedException();
        }

        private void FinishLevel()
        {
            throw new NotImplementedException();
        }
    }
}
