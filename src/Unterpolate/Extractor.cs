using System;
using System.Collections.Generic;

namespace Knapcode.Unterpolate
{
    public static class Extractor
    {
        public static ExtractResult Extract(string input, string pattern)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var tokens = Tokenizer.Tokenize(pattern);
            var parsed = Parser.Parse(tokens);
            var match = parsed.Pattern.Match(input);

            if (!match.Success)
            {
                return ExtractResult.Unsuccessful;
            }

            var values = new List<string>();
            var nameToValue = new Dictionary<string, string>();
            for (var i = 0; i < parsed.Captures.Count; i++)
            {
                var capture = parsed.Captures[i];
                var value = match.Groups[i + 1].Value;
                values.Add(value);
                if (capture.Name != null)
                {
                    nameToValue[capture.Name] = value;
                }
            }

            return new ExtractResult(success: true, values, nameToValue);
        }
    }
}
