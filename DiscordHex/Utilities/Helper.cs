using System.Text;
using System.Text.RegularExpressions;

namespace DiscordHex.Utilities
{
    internal static class Helper
    {
        internal static string Pad(string content, string padding, int length)
        {
            if (content.Length >= length)
                return content;

            var sb = new StringBuilder(content);

            while (sb.Length < length)
                sb.Append(padding);

            return sb.ToString();
        }
    }
}
