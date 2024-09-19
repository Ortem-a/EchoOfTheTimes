using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EchoOfTheTimes.SceneManagement
{
    public partial class SceneGroupManager
    {
        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };

        private GameLevel _activeSceneGroup;

        public async Task LoadScenesAsync(GameLevel group, IProgress<float> progress, bool reloadDuplicateScenes = false)
        {
            _activeSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenesAcync();

            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = _activeSceneGroup.Scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (int i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.Scenes[i];

                if (!reloadDuplicateScenes && loadedScenes.Contains(sceneData.Name)) continue;

                var operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);

                operationGroup.Operations.Add(operation);

                OnSceneLoaded.Invoke(sceneData.Name);
            }

            // wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }

            OnSceneGroupLoaded.Invoke();
        }

        public async Task UnloadScenesAcync()
        {
            var scenes = new List<string>();

            var activeScene = SceneManager.GetActiveScene().name;
            int sceneCount = SceneManager.sceneCount;

            for (int i = sceneCount - 1; i > 0; i--)
            {
                var sceneAt = SceneManager.GetSceneAt(i);

                if (!sceneAt.isLoaded) continue;

                var sceneName = sceneAt.name;

                //if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
                if (sceneName == "Bootstrapper") continue;

                scenes.Add(sceneName);
            }

            var operationGroup = new AsyncOperationGroup(scenes.Count);

            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);

                if (operation == null) continue;

                operationGroup.Operations.Add(operation);

                OnSceneUnloaded.Invoke(scene);
            }

            // wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }

            // unload all unused assets from memory
            Resources.UnloadUnusedAssets();
        }
    }
}