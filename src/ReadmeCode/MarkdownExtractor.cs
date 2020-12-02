using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Knapcode.ReadmeCode
{
    public static class MarkdownExtractor
    {
        public static IEnumerable<MarkdownRegion> ReadMarkdownCodeRegions(string path)
        {
            using var fileStream = File.OpenRead(path);
            using var reader = new StreamReader(fileStream);
            var fileContent = reader.ReadToEnd();

            var codeRegions = GetCodeRegions(fileContent);
            var commandOutputs = GetCommandOutputs(fileContent);

            return Enumerable
                .Empty<MarkdownRegion>()
                .Concat(codeRegions)
                .Concat(commandOutputs);
        }

        private static IEnumerable<MarkdownCodeRegion> GetCodeRegions(string fileContent)
        {
            var regions = GetRegions(fileContent, "CODE-REGION");

            foreach (var region in regions)
            {
                var splits = region
                    .parameters
                    .Split(' ')
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0)
                    .ToList();

                yield return new MarkdownCodeRegion(region.content, splits[0], splits[1]);
            }
        }

        private static IEnumerable<MarkdownCommandOuput> GetCommandOutputs(string fileContent)
        {
            return GetRegions(fileContent, "COMMAND-OUTPUT")
                .Select(x => new MarkdownCommandOuput(x.content, x.parameters));
        }

        private static IEnumerable<(string parameters, string content)> GetRegions(string fileContent, string tagName)
        {
            var regionStartPattern = new Regex(@$"<!--\s*{Regex.Escape(tagName)}(?<Parameters>\s+.+?)?-->", RegexOptions.Singleline);
            var regionEndPattern = new Regex(@$"<!--\s/{Regex.Escape(tagName)}\s*-->", RegexOptions.Singleline);

            var startAt = 0;
            while (true)
            {
                var startMatch = regionStartPattern.Match(fileContent, startAt);
                if (!startMatch.Success)
                {
                    break;
                }

                startAt = startMatch.Index + startMatch.Length;

                var endMatch = regionEndPattern.Match(fileContent, startAt);
                if (!endMatch.Success)
                {
                    break;
                }

                var parametersGroup = startMatch.Groups["Parameters"];
                var parameters = string.Empty;
                if (parametersGroup.Success)
                {
                    parameters = parametersGroup.Value.Trim();
                }

                var content = fileContent.Substring(startAt, endMatch.Index - startAt);
                var trimmedContent = TrimCodeFence(content);

                yield return (parameters, trimmedContent);

                startAt = endMatch.Index + endMatch.Length;
            }
        }

        private static string TrimCodeFence(string content)
        {
            var trimmedContent = content.Trim();
            if (trimmedContent.StartsWith("```", StringComparison.Ordinal) && trimmedContent.EndsWith("```", StringComparison.Ordinal))
            {
                var endOfFirstLine = trimmedContent.IndexOfAny(StringUtility.LineEndingChars);
                var startOfLastLine = trimmedContent.LastIndexOfAny(StringUtility.LineEndingChars);
                if (endOfFirstLine > 0 && startOfLastLine > endOfFirstLine)
                {
                    trimmedContent = trimmedContent.Substring(endOfFirstLine, startOfLastLine - endOfFirstLine);
                    trimmedContent = trimmedContent.Trim(StringUtility.LineEndingChars);
                    return trimmedContent;
                }
            }

            return content;
        }
    }
}
