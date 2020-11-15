using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Knapcode.Unterpolate
{
    internal class ParsedPattern
    {
        public ParsedPattern(Regex pattern, IReadOnlyList<ParsedCapture> captures) => (Pattern, Captures) = (pattern, captures);

        public Regex Pattern { get; }
        public IReadOnlyList<ParsedCapture> Captures { get; }
    }
}
