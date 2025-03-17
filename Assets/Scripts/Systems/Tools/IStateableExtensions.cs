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

            foreach (var keyValuePair in stateable.Options)
            {
                sb.Append($"[{keyValuePair.Key}]\n{keyValuePair.Value}");
            }

            return sb.ToString();
        }
    }
}