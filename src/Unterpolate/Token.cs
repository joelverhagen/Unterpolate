using System;
using System.Collections.Generic;

namespace Knapcode.Unterpolate
{
    internal class Token : IEquatable<Token>
    {
        public Token(TokenType type, string value) => (Type, Value) = (type, value);
        public TokenType Type { get; }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Token);
        }

        public bool Equals(Token other)
        {
            return other != null &&
                   Type == other.Type &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = 1265339359;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
