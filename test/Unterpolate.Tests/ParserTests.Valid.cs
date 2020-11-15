using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Knapcode.Unterpolate
{
    public partial class ParserTests
    {
        [Theory]
        [MemberData(nameof(ValidTestData))]
        internal void ParsesValidInput(string input)
        {
            var tokens = Tokenizer.Tokenize(input).ToList();
            var testCase = InputToValidTestCase[input];

            var actual = Parser.Parse(tokens);

            Assert.Equal(testCase.Pattern, actual.Pattern.ToString());
            Assert.Equal(testCase.Captures.ToList(), actual.Captures);

            Assert.Equal(RegexOptions.Singleline, actual.Pattern.Options);
            Assert.Equal(Regex.InfiniteMatchTimeout, actual.Pattern.MatchTimeout);
            Assert.False(actual.Pattern.RightToLeft);
        }

        public static IEnumerable<object[]> ValidTestData => ValidTestCases
            .Select(x => new object[] { x.Input });

        private static IReadOnlyDictionary<string, ValidTestCase> InputToValidTestCase => ValidTestCases
            .ToDictionary(x => x.Input);

        private static IEnumerable<ValidTestCase> ValidTestCases => new[]
        {
            new ValidTestCase
            {
                Input = "foo {} bar",
                Pattern = @"foo\ (.+)\ bar",
                Captures = new[]
                {
                    new ParsedCapture(string.Empty),
                },
            },
            new ValidTestCase
            {
                Input = "foo {bar} baz",
                Pattern = @"foo\ (.+)\ baz",
                Captures = new[]
                {
                    new ParsedCapture("bar"),
                },
            },
            new ValidTestCase
            {
                Input = "foo {   bar } baz",
                Pattern = @"foo\ (.+)\ baz",
                Captures = new[]
                {
                    new ParsedCapture("   bar "),
                },
            },
            new ValidTestCase
            {
                Input = "{}{}",
                Pattern = @"(.+)(.+)",
                Captures = new[]
                {
                    new ParsedCapture(string.Empty),
                    new ParsedCapture(string.Empty),
                },
            },
            new ValidTestCase
            {
                Input = "{foo}{bar}",
                Pattern = @"(.+)(.+)",
                Captures = new[]
                {
                    new ParsedCapture("foo"),
                    new ParsedCapture("bar"),
                },
            },
            new ValidTestCase
            {
                Input = " {}  {}  {} ",
                Pattern = @"\ (.+)\ \ (.+)\ \ (.+)\ ",
                Captures = new[]
                {
                    new ParsedCapture(string.Empty),
                    new ParsedCapture(string.Empty),
                    new ParsedCapture(string.Empty),
                },
            },
        };

        private record ValidTestCase
        {
            public string Input { get; init; }
            public string Pattern { get; init; }
            public IReadOnlyList<ParsedCapture> Captures { get; init; }
        }
    }
}
