using System;
using System.Collections.Generic;

namespace Knapcode.Unterpolate
{
    internal class ParsedCapture : IEquatable<ParsedCapture>
    {
        public ParsedCapture(string name) => Name = name;

        public string Name { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParsedCapture);
        }

        public bool Equals(ParsedCapture other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
