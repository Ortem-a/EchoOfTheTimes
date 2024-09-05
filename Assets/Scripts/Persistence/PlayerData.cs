using System.Collections.Generic;

namespace EchoOfTheTimes.Persistence
{
    [System.Serializable]
    public class PlayerData
    {
        public List<int> OpenedLevelIds = new List<int>() { 0 };

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (int id in OpenedLevelIds)
            {
                sb.Append($"{id}, ");
            }

            return $"Opened Levels ({OpenedLevelIds.Count}) Ids: <{sb}>";
        }
    }
}