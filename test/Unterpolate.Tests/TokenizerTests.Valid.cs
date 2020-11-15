using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Knapcode.Unterpolate
{
    public partial class TokenizerTests
    {
        [Theory]
        [MemberData(nameof(ValidTestData))]
        internal void TokenizesValidInput(string input)
        {
            var expected = InputToValidTestCase[input].Tokens.ToList();

            var actual = Tokenizer.Tokenize(input).ToList();
            
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> ValidTestData => ValidTestCases
            .Select(x => new object[] { x.Input });

        private static IReadOnlyDictionary<string, ValidTestCase> InputToValidTestCase => ValidTestCases
            .ToDictionary(x => x.Input);

        private static IEnumerable<ValidTestCase> ValidTestCases => new[]
        {
            new ValidTestCase
            {
                Input = "{}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, string.Empty),
                    new Token(TokenType.CaptureName, string.Empty),
                },
            },
            new ValidTestCase
            {
                Input = "{}{}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, string.Empty),
                    new Token(TokenType.CaptureName, string.Empty),
                    new Token(TokenType.Literal, string.Empty),
                    new Token(TokenType.CaptureName, string.Empty),
                },
            },
            new ValidTestCase
            {
                Input = "{} {}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, string.Empty),
                    new Token(TokenType.CaptureName, string.Empty),
                    new Token(TokenType.Literal, " "),
                    new Token(TokenType.CaptureName, string.Empty),
                },
            },
            new ValidTestCase
            {
                Input = "foo {}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo "),
                    new Token(TokenType.CaptureName, string.Empty),
                },
            },
            new ValidTestCase
            {
                Input = "{} foo",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, string.Empty),
                    new Token(TokenType.CaptureName, string.Empty),
                    new Token(TokenType.Literal, " foo"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {} bar",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo "),
                    new Token(TokenType.CaptureName, string.Empty),
                    new Token(TokenType.Literal, " bar"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {bar} baz",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo "),
                    new Token(TokenType.CaptureName, "bar"),
                    new Token(TokenType.Literal, " baz"),
                },
            },
            new ValidTestCase
            {
                Input = "foo { bar   } baz",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo "),
                    new Token(TokenType.CaptureName, " bar   "),
                    new Token(TokenType.Literal, " baz"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {bar} {baz} qux",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo "),
                    new Token(TokenType.CaptureName, "bar"),
                    new Token(TokenType.Literal, " "),
                    new Token(TokenType.CaptureName, "baz"),
                    new Token(TokenType.Literal, " qux"),
                },
            },
            new ValidTestCase
            {
                Input = "{{",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "{"),
                },
            },
            new ValidTestCase
            {
                Input = "}}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "}"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {{bar}}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo {bar}"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {{}} bar",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo {} bar"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {{{bar}}}",
                Tokens = new[]
                {
                    new Token(TokenType.Literal, "foo {"),
                    new Token(TokenType.CaptureName, "bar"),
                    new Token(TokenType.Literal, "}"),
                },
            },
        };

        private record ValidTestCase
        {
            public string Input { get; init; }
            public IReadOnlyList<Token> Tokens { get; init; }
        }
    }
}
