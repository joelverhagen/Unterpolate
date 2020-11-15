using System;
using System.Collections.Generic;

namespace Knapcode.Unterpolate
{
    internal static class Tokenizer
    {
        public static IEnumerable<Token> Tokenize(string text)
        {
            var tokenStart = 0;
            var tokenType = TokenType.Literal;

            var reader = new CharReader(text);
            while (!reader.IsAtEnd)
            {
                TokenType nextTokenType;
                int tokenEnd;

                switch (tokenType)
                {
                    case TokenType.Literal:
                        nextTokenType = ReadLiteral(reader);
                        tokenEnd = reader.Position;
                        break;
                    case TokenType.CaptureName:
                        tokenStart++;
                        nextTokenType = ReadCaptureName(reader);
                        tokenEnd = reader.Position - 1;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                var value = text.Substring(tokenStart, tokenEnd - tokenStart);
                value = value.Replace("{{", "{").Replace("}}", "}");
                yield return new Token(tokenType, value);
                tokenType = nextTokenType;
                tokenStart = reader.Position;
            }
        }

        private static TokenType ReadLiteral(CharReader reader)
        {
            while (!reader.IsAtEnd)
            {
                switch (reader.Current)
                {
                    case '{':
                        if (reader.Next == '{')
                        {
                            reader.MoveNext();
                            reader.MoveNext();
                            continue;
                        }

                        return TokenType.CaptureName;
                    case '}':
                        if (reader.Next == '}')
                        {
                            reader.MoveNext();
                            reader.MoveNext();
                            continue;
                        }
                        else
                        {
                            throw new FormatException($"At index {reader.Position}, there is an unescaped '}}' character.");
                        }
                }

                reader.MoveNext();
            }

            return TokenType.Literal;
        }

        private static TokenType ReadCaptureName(CharReader reader)
        {
            var startPosition = reader.Position;
            reader.MoveNext();

            while (!reader.IsAtEnd)
            {
                switch (reader.Current)
                {
                    case '{':
                        throw new FormatException($"At index {reader.Position}, there is an unexpected '{{' character.");
                    case '}':
                        reader.MoveNext();
                        return TokenType.Literal;
                }

                reader.MoveNext();
            }

            throw new FormatException($"There is a capture section starting at index {startPosition} without a terminating '}}' character.");
        }

        private class CharReader
        {
            private const char Invalid = char.MaxValue;

            public CharReader(string text)
            {
                Text = text;
                Position = 0;
            }

            public string Text { get; }
            public int Position { get; private set; }
            public bool IsAtEnd => Position >= Text.Length;

            public void MoveNext()
            {
                Position++;
            }

            public char Next
            {
                get
                {
                    if (Position + 1 >= Text.Length)
                    {
                        return Invalid;
                    }

                    return Text[Position + 1];
                }
            }

            public char Current
            {
                get
                {
                    if (IsAtEnd)
                    {
                        return Invalid;
                    }

                    return Text[Position];
                }
            }
        }
    }
}
