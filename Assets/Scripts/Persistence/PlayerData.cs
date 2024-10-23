using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;

namespace EchoOfTheTimes.Persistence
{
    [System.Serializable]
    public class PlayerData
    {
        public bool SoundsMuted;

        public string LastLoadedLevelFullName = string.Empty;

        public List<GameChapter> Data;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append($"Settings:\n\t{nameof(SoundsMuted)}: {SoundsMuted}\n");
            sb.Append($"{nameof(LastLoadedLevelFullName)}: {LastLoadedLevelFullName}\n");

            foreach (var chapter in Data)
            {
                sb.Append($"{chapter}\n===============\n");
            }

            return $"Player Data: <{sb}>";
        }
    }
}