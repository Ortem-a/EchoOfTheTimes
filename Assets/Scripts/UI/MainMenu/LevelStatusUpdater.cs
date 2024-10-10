using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class LevelStatusUpdater : MonoBehaviour
    {
        private PersistenceService _persistenceService;

        private LevelButtonView[] _views;
        private LevelButtonHandler[] _handlers;
        private List<GameLevel> _levelsData;

        [SerializeField]
        private string _chapterTitle;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _persistenceService = mainMenuService.PersistenceService;
        }

        private void Awake()
        {
            _levelsData = _persistenceService.GetData()
                .Find((chapter) => chapter.Title == _chapterTitle).Levels;

            _views = GetComponentsInChildren<LevelButtonView>();
            _handlers = new LevelButtonHandler[_views.Length];
            for (int i = 0; i < _views.Length; i++)
            {
                _handlers[i] = _views[i].GetComponent<LevelButtonHandler>();
            }
        }

        private void Start()
        {
            UpdateLevelsData();
        }

        private void UpdateLevelsData()
        {
            for (int i = 0; i < _views.Length; i++)
            {
                _views[i].UpdateData(_levelsData[i]);
                _handlers[i].SetData(_levelsData[i]);
            }
        }
    }
}