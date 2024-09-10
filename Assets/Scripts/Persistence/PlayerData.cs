using System.Collections.Generic;

namespace EchoOfTheTimes.Persistence
{
    [System.Serializable]
    public class PlayerData
    {
        public List<int> OpenedLevels = new List<int>() { 0 };

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (int id in OpenedLevels)
            {
                sb.Append($"{id}, ");
            }

            return $"Opened Levels ({OpenedLevels.Count}) Ids: <{sb}>";
        }
    }
}