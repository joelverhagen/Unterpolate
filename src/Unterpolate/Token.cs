namespace Knapcode.Unterpolate
{
    internal class Token
    {
        public Token(TokenType type, string value) => (Type, Value) = (type, value);
        public TokenType Type { get; }
        public string Value { get; }
    }
}
