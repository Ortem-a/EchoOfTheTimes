using System;
using UnityEngine;

namespace Systems.Core
{
    public class GameLoopService
    {
        public event Action OnLevelStarted;
        public event Action OnLevelFinished;

        public void StartLevel()
        {
            Debug.Log($"[GameLoopService] START LEVEL");

            OnLevelStarted?.Invoke();
        }

        public void FinishLevel()
        {
            Debug.Log($"[GameLoopService] FINISH LEVEL");

            OnLevelFinished?.Invoke();

            // run scene manager here
        }
    }
}