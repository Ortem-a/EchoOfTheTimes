using System.IO;
using UnityEngine;

namespace EchoOfTheTimes.Persistence
{
    public class SaveLoadService
    {
        private readonly string _fileName = "SaveData.json";
        private readonly string _pathToFile;

        public PlayerData DataToSave { get; private set; }

        public SaveLoadService()
        {
            _pathToFile = Path.Combine(Application.dataPath, _fileName);

            if (File.Exists(_pathToFile))
            {
                Load();
            }
            else
            {
                Debug.LogWarning($"There is no file with saves! Generate file... '{_pathToFile}'");

                DataToSave = new PlayerData();
                Save();
            }
        }

        public void Save()
        {
            File.WriteAllText(_pathToFile, JsonUtility.ToJson(DataToSave));

            Debug.Log($"Successfully save data: {DataToSave}");
        }

        public void Load()
        {
            DataToSave = JsonUtility.FromJson<PlayerData>(File.ReadAllText(_pathToFile));

            Debug.Log($"Successfully load data: {DataToSave}");
        }
    }
}