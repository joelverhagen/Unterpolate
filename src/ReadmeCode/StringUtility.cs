using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Knapcode.ReadmeCode
{
    public static class StringUtility
    {
        internal static readonly char[] Indentation = new[] { ' ', '\t' };
        internal static readonly char[] LineEndingChars = new[] { '\n', '\r' };
        internal static readonly string[] LineEndings = new[] { "\r\n", "\n", "\r" };

        public static string TrimIndentation(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var lines = input
                .TrimEnd()
                .Split(LineEndings, StringSplitOptions.None)
                .Select(x => x.TrimEnd())
                .SkipWhile(l => l.Length == 0)
                .ToList();

            var nonEmptyLines = lines
                .Where(l => l.Length > 0)
                .ToList();

            var prefix = LongestCommonPrefix(nonEmptyLines);
            var indentationLength = prefix.Length - prefix.TrimStart(Indentation).Length;
            var trimmedLines = lines.Select(l => l.Length > 0 ? l.Substring(indentationLength) : l);
            return string.Join(Environment.NewLine, trimmedLines);
        }

        public static string LongestCommonPrefix(IReadOnlyList<string> strings)
        {
            if (strings == null)
            {
                throw new ArgumentNullException(nameof(strings));
            }

            if (strings.Count == 0)
            {
                return string.Empty;
            }

            if (strings.Count == 1)
            {
                return strings[0];
            }

            strings = strings.OrderBy(x => x).ToList();
            var first = strings[0];
            var last = strings[strings.Count - 1];
            var minLength = Math.Min(last.Length, last.Length);

            int prefix;
            for (prefix = 0; prefix < minLength && first[prefix] == last[prefix]; prefix++)
            {
            }

            return first.Substring(0, prefix);
        }
    }
}
