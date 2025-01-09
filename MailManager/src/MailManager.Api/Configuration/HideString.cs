using System.Text.RegularExpressions;

namespace MailManager.Api.Configuration
{
    public static class HideString
    {
        public static string ToHide(this string source)
        => !string.IsNullOrEmpty(source) ? $"#####{Regex.Replace(source, "[vabcaeiou/:.123]", "#").Substring(5)}" : source;
    }
}
