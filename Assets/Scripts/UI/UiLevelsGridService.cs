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

            for (int i = 1; i < _loader.SceneGroups.Length; i++)
            {
                CreateButton(i);
            }
        }

        private void CreateButton(int index)
        {
            var obj = Instantiate(ButtonPrefab, ButtonsContainer);

            obj.transform.GetChild(0).GetComponent<TMP_Text>().text = _loader.SceneGroups[index].GroupName;
            obj.GetComponent<Button>().onClick.AddListener(() => LoadSceneGroup(index));
        }

        private async void LoadSceneGroup(int index)
        {
            await _loader.LoadSceneGroupAsync(index);
        }
    }
}