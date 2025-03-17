using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.Core.SceneManagement
{
    public class SceneLoader
    {
        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };

        private GameLevel _activeScene;

        public async Task LoadSceneAsync(GameLevel level, IProgress<float> progress)
        {
            _activeScene = level;

            var operation = SceneManager.LoadSceneAsync(_activeScene.Scene.Reference.Path, LoadSceneMode.Additive);

            while (!operation.isDone)
            {
                progress?.Report(operation.progress);
                await Task.Delay(100);
            }

            OnSceneLoaded.Invoke(_activeScene.Scene.Name);

            Scene activeScene = SceneManager.GetSceneByName(_activeScene.Scene.Name);

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
        }

        public async Task UnloadSceneAsync()
        {
            if (_activeScene == null) return;

            var operation = SceneManager.UnloadSceneAsync(_activeScene.Scene.Reference.Path);

            while (!operation.isDone)
            {
                await Task.Delay(100);
            }

            OnSceneUnloaded.Invoke(_activeScene.Scene.Name);

            operation = Resources.UnloadUnusedAssets();

            while (!operation.isDone)
            {
                await Task.Delay(100);
            }
        }
    }
}