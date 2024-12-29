using System.Text;
using Systems.Leveling;

namespace Systems.Tools
{
    public static class IStateableExtensions
    {
        public static string OptionsToString(this IStateable stateable)
        {
            if (stateable.Options == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < stateable.Options.Length; i++)
            {
                sb.Append($"[{i}]\n{stateable.Options[i]}");
            }

            return sb.ToString();
        }
    }
}