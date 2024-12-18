using System.Collections.Generic;
using System.IO;
using System.Linq;
using Systems.Leveling;
using UnityEngine;

namespace Systems.Tools
{
    public static class SceneSaver
    {
        private static readonly string _extension = ".json";
        private static readonly string _pathToSavedData = Path.Combine(Application.dataPath, "..");

        private static Dictionary<string, List<IStateable>> _data = new Dictionary<string, List<IStateable>>();

        public static void Save(string sceneName, IStateable stateable)
        {
            var stateables = Object.FindObjectsByType<Stateable>(FindObjectsSortMode.None);

            if (_data.ContainsKey(sceneName))
            {
                _data[sceneName] = stateables.ToList<IStateable>();
            }
            else
            {
                _data.Add(sceneName, stateables.ToList<IStateable>());
            }

            SaveDataToJson(sceneName);
        }

        private static void SaveDataToJson(string fileName)
        {
            var fullPath = Path.Combine(_pathToSavedData, fileName + _extension);
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }

            var data = JsonUtility.ToJson(_data, true);

            File.WriteAllText(fullPath, data);
        }
    }
}