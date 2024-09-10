using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    public class UiLevelsGridService : MonoBehaviour
    {
        public GameObject ButtonPrefab;
        public Transform ButtonsContainer;

        private SceneLoader _loader;

        private void Start()
        {
            _loader = FindObjectOfType<SceneLoader>();

            for (int i = 1; i < _loader.GameChapters.Count; i++)
            {
                //CreateButton(i);
                CreateChapter(i);
            }
        }

        private void CreateChapter(int chapterIndex)
        {
            // add chapter title
            // _loader.GameChapters[chapterIndex].Title

            for (int i = 0; i < _loader.GameChapters[chapterIndex].Levels.Count; i++)
            {
                CreateButton(_loader.GameChapters[chapterIndex].Levels[i], ButtonsContainer);
            }
        }

        private void CreateButton(GameLevel level, Transform container)
        {
            var obj = Instantiate(ButtonPrefab, container);

            obj.transform.GetChild(0).GetComponent<TMP_Text>().text = level.LevelName;// _loader.GameChapters[index].GroupName;
            //obj.GetComponent<Button>().onClick.AddListener(() => LoadSceneGroup(index));
            obj.GetComponent<Button>().onClick.AddListener(() => LoadSceneGroup(level));
        }

        private async void LoadSceneGroup(GameLevel level)
        {
            await _loader.LoadSceneGroupAsync(level);
        }

        //private async void LoadSceneGroup(int index)
        //{
        //    await _loader.LoadSceneGroupAsync(index);
        //}
    }
}