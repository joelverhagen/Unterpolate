using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Knapcode.ReadmeCode
{
    public static class CodeExtractor
    {
        public static IReadOnlyDictionary<string, string> ReadCodeRegions(string path)
        {
            using var fileStream = File.OpenRead(path);
            using var reader = new StreamReader(fileStream);
            var content = reader.ReadToEnd();

            var regionStartPattern = new Regex(@"^ *#region\s+(.+)", RegexOptions.Multiline);
            var regionEndPattern = new Regex("#endregion");

            var output = new Dictionary<string, string>();
            var startAt = 0;
            while (true)
            {
                var startMatch = regionStartPattern.Match(content, startAt);
                if (!startMatch.Success)
                {
                    break;
                }

                startAt = startMatch.Index + startMatch.Length;

                var endMatch = regionEndPattern.Match(content, startAt);
                if (!endMatch.Success)
                {
                    break;
                }

                var regionName = startMatch.Groups[1].Value.Trim();
                var regionContent = content.Substring(startAt, endMatch.Index - startAt);

                output[regionName] = regionContent;

                startAt = endMatch.Index + endMatch.Length;
            }

            return output;
        }
    }
}
