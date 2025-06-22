using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Systems.Core
{
    public class LevelEntryPoint : IInitializable, IDisposable
    {
        private readonly GameLoopService _gameLoop;

        public LevelEntryPoint(GameLoopService gameLoop)
        {
            _gameLoop = gameLoop;
        }

        void IInitializable.Initialize()
        {
            // level loaded
            // prepare UI 
            // show start splash screen
            // when splash screen animation finished -> OnLevelStarted.Invoke()
        }

        void IDisposable.Dispose() 
        {

        }
    }
}
