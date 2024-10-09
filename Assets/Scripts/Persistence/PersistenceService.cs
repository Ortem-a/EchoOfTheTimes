using EchoOfTheTimes.SceneManagement;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.ScriptableObjects.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Persistence
{
    public class PersistenceService : MonoBehaviour
    {
        public static Action OnLevelCompleted { get; private set; }
        public static Action OnExitToMainMenu { get; private set; }

        private SaveLoadService _saveLoadService;

        private GameLevel _lastLoadedLevel;

        private PresetType _presetType;

        private PlayerData _defaultData;
        private PlayerData _allUnlockData;

        private void OnDestroy()
        {
            OnLevelCompleted -= HandleLevelCompleted;
            OnExitToMainMenu -= HandleExitToMainMenu;
        }

        [Inject]
        private void Construct()
        {
            OnLevelCompleted += HandleLevelCompleted;
            OnExitToMainMenu += HandleExitToMainMenu;

            LoadPresets();

            _presetType = (Resources.Load(@"ScriptableObjects/BootstrapSettings") as BootstrapSettingsScriptableObject).UsedSavingPreset;

            _saveLoadService = _presetType switch
            {
                PresetType.SavedFile => new SaveLoadService(_defaultData),
                PresetType.Default => new SaveLoadService(_defaultData, true),
                PresetType.AllUnlock => new SaveLoadService(_allUnlockData, true),
                _ => throw new NotImplementedException($"Not implemented {nameof(PresetType)} with value: '{_presetType}'!"),
            };

            Debug.Log($"Enable {nameof(_saveLoadService)} with preset: {_presetType}!");
        }

        private void LoadPresets()
        {
            _defaultData = LoadPresetFromResources(@"ScriptableObjects/Persistence/PlayerDataDefaultPreset-AutoGenerated");
            _allUnlockData = LoadPresetFromResources(@"ScriptableObjects/Persistence/PlayerDataUnlockedAllPreset-AutoGenerated");
        }

        private PlayerData LoadPresetFromResources(string path)
        {
            var preset = Resources.LoadAsync(path).asset as PlayerDataPresetScriptableObject;

            if (preset == null)
            {
                throw new NullReferenceException($"You need to create '{path.Split(Path.PathSeparator)[^1]}' first!");
            }

            return preset.Data;
        }

        private void HandleLevelCompleted()
        {
            var newDataToSave = _saveLoadService.DataToSave;

#warning can be if U start NOT from MAIN MENU
            _lastLoadedLevel ??= _saveLoadService.DataToSave.Data[1].Levels[0];

            // �������� ������� ������� ��� ����������
            var lastLoadedChapterTitle = _lastLoadedLevel.ChapterName;
            var lastLoadedLevelName = _lastLoadedLevel.LevelName;
            int lastLoadedChapterIndex = -1;
            int lastLoadedLevelIndex = -1;
            for (int i = 0; i < newDataToSave.Data.Count; i++)
            {
                if (newDataToSave.Data[i].Title == lastLoadedChapterTitle)
                {
                    var lastLoadedChapter = newDataToSave.Data[i];
                    lastLoadedChapterIndex = i;
                    for (int j = 0; j < lastLoadedChapter.Levels.Count; j++)
                    {
                        if (lastLoadedChapter.Levels[j].LevelName == lastLoadedLevelName)
                        {
                            lastLoadedChapter.Levels[j].LevelStatus = StatusType.Completed;
                            lastLoadedLevelIndex = j;
                            break;
                        }
                    }

                    break;
                }
            }

            // �������� ��������� ��� ��������
            // �������� ������:
            // ���� ������� � ������ ��� � �������� �����
            // ���� ��������� ������� �����
            // ���� ��������� ������� ��������� �����

            // ���� �� ��������� ������� �� �����
            if (lastLoadedLevelIndex < newDataToSave.Data[lastLoadedChapterIndex].Levels.Count - 1)
            {
                lastLoadedLevelIndex++;
                newDataToSave.Data[lastLoadedChapterIndex].Levels[lastLoadedLevelIndex].LevelStatus = StatusType.Unlocked;
            }
            else
            {
                // ���� �� ��������� �����
                if (lastLoadedChapterIndex < newDataToSave.Data.Count - 1)
                {
                    newDataToSave.Data[lastLoadedChapterIndex].ChapterStatus = StatusType.Completed;

                    lastLoadedChapterIndex++;
                    newDataToSave.Data[lastLoadedChapterIndex].ChapterStatus = StatusType.Unlocked;
                    newDataToSave.Data[lastLoadedChapterIndex].Levels[0].LevelStatus = StatusType.Unlocked;
                }
                else
                {
                    newDataToSave.Data[lastLoadedChapterIndex].ChapterStatus = StatusType.Completed;
                }
            }

            _lastLoadedLevel = newDataToSave.Data[lastLoadedChapterIndex].Levels[lastLoadedLevelIndex];

            // ��������� ������
            if (_presetType == PresetType.SavedFile)
            {
                _saveLoadService.DataToSave = newDataToSave;
                _saveLoadService.Save();
            }
        }

        private void HandleExitToMainMenu()
        {
            // ��������� ������
            if (_presetType == PresetType.SavedFile)
            {
                _saveLoadService.Save();
            }
        }

        public List<GameChapter> GetData() => _saveLoadService.DataToSave.Data;

        public void UpdateLastLoadedLevel(GameLevel level) => _lastLoadedLevel = level;

        public GameLevel GetLastLoadedLevel() => _lastLoadedLevel;
    }
}