using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Knapcode.Unterpolate
{
    internal static class Parser
    {
        private const string LazyCaptureAllRaw = "(.+)";

        public static ParsedPattern Parse(IEnumerable<Token> tokens)
        {
            var builder = new List<string>();
            var captures = new List<ParsedCapture>();
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Literal:
                        builder.Add(Regex.Escape(token.Value));
                        break;
                    case TokenType.CaptureName:
                        builder.Add(LazyCaptureAllRaw);
                        captures.Add(new ParsedCapture(token.Value));
                        break;
                }
            }

            var pattern = new Regex(string.Join(string.Empty, builder), RegexOptions.Singleline);

            return new ParsedPattern(pattern, captures);
        }
    }
}
