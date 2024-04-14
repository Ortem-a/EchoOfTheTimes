using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Units;
using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EchoOfTheTimes.Persistence
{
    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        public GameData GameData;

        private IDataService _dataService;

        protected override void Awake()
        {
            base.Awake();

            _dataService = new FileDataService(new JsonSerializer());
        }

        //private void Start() => SceneManager.sceneLoaded += OnSceneLoaded;
        //private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    Bind<Player, PlayerData>(GameData.PlayerData);
        //}

        public void BindPlayer()
        {
            Bind<Player, PlayerData>(GameData.PlayerData);
        }

        private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();

            if (entity != null)
            {
                if (data == null)
                {
                    data = new TData() { Id = entity.Id };
                }

                entity.Bind(data);
            }
        }

        private void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(d => d.Id == entity.Id);

                if (data == null)
                {
                    data = new TData() { Id = entity.Id };
                    datas.Add(data);
                }

                entity.Bind(data);
            }
        }

        public void NewGame()
        {
            GameData = new GameData()
            {
                Name = "New Game",
                CurrentLevelName = "CodeTests"
            };

            SceneManager.LoadScene(GameData.CurrentLevelName);
        }

        public void LoadGame(string gameName)
        {
            GameData = _dataService.Load(gameName);

            if (string.IsNullOrWhiteSpace(gameName))
            {
                GameData.CurrentLevelName = "CodeTests";
            }

#warning ИВАН ГОВНОВ
            // здесь должен быть 1ый чекпоинт на уровне.
            // он загружается разрабом на диск, потом только загружается.
            // пользователь не может менять его, сохранять и перезаписывать.
            // фактически пользователь не может ничего сохранять. он терпила
            //var vertex = GameManager.Instance.Graph.GetNearestVertex(GameData.PlayerData.Checkpoint);
            //GameManager.Instance.CheckpointManager.OnCheckpointChanged?.Invoke(vertex.gameObject.GetComponent<Checkpoint>());
            //GameManager.Instance.StateMachine.LoadState(GameData.PlayerData.StateId);
            //GameManager.Instance.Player.Teleportate(GameData.PlayerData.Checkpoint, 0f);
        }

        public void SaveGame(PlayerData playerData)
        {
            GameData = new GameData()
            {
                Name = "TestSave",
                CurrentLevelName = SceneManager.GetActiveScene().name,
                PlayerData = playerData
            };

            SaveGame();
        }

        public void SaveGame() => _dataService.Save(GameData);

        public void DeleteGame(string gameName) => _dataService.Delete(gameName);

        public void ReloadGame() => LoadGame(GameData.Name);
    }
}