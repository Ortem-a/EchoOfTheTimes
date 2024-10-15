using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EchoOfTheTimes.Collectables.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CollectableSpawner))]
    public class CollectableSpawnerEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var spawner = (CollectableSpawner)target;

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("USE ONLY BUTTON FOR ADDING PLACEHOLDERS", MessageType.Warning);

            if (GUILayout.Button("Find All Placeholders"))
            {
                var placeholders = FindObjectsOfType<CollectablePlaceholder>();

                spawner.SetPlaceholdersFromEditor(placeholders.ToList());

                var service = spawner.GetComponent<CollectableService>();

                service.SetTotalCollectables(placeholders.Length);

                var sceneLoader = FindObjectOfType<SceneLoader>();

                Scene scene = default;
                bool needToClose = false;
                if (sceneLoader == null)
                {
                    needToClose = true;
                    scene = EditorSceneManager.OpenScene(@"Assets/Scenes/Bootstrapper.unity", OpenSceneMode.Additive);

                    sceneLoader = FindObjectOfType<SceneLoader>();
                }

                var chapterIndex = sceneLoader.GameChapters.FindIndex((chapter) => chapter.Title == service.ChapterTitle);
                var levelIndex = sceneLoader.GameChapters[chapterIndex].Levels.FindIndex((level) => level.LevelName == service.LevelName);

                sceneLoader.GameChapters[chapterIndex].Levels[levelIndex].TotalCollectables = placeholders.Length;

                if (needToClose)
                {
                    EditorSceneManager.SaveScene(scene);
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }
    }
#endif
}