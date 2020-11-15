using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Knapcode.Unterpolate
{
    public partial class ExtractorTests
    {
        [Theory]
        [MemberData(nameof(ValidSuccessfulTestData))]
        public void ExtractsDataWhenPatternMatches(string input, string pattern)
        {
            var testCase = InputToValidTestCase[(input, pattern)];

            var result = Extractor.Extract(input, pattern);

            Assert.True(result.Success);
            Assert.Equal(testCase.Values.ToList(), result.ToList());
            Assert.Equal(testCase.NameToValue, result.NameToValue);
        }

        [Theory]
        [MemberData(nameof(ValidUnsuccessfulTestData))]
        public void DoesNotExtractDataWhenPatternDoesNotMatch(string input, string pattern)
        {
            var result = Extractor.Extract(input, pattern);

            Assert.False(result.Success);
            Assert.Empty(result);
            Assert.Empty(result.NameToValue);
        }

        public static IEnumerable<object[]> ValidUnsuccessfulTestData => ValidTestCases
            .Where(x => x.Values == null)
            .Select(x => new object[] { x.Input, x.Pattern });

        public static IEnumerable<object[]> ValidSuccessfulTestData => ValidTestCases
            .Where(x => x.Values != null)
            .Select(x => new object[] { x.Input, x.Pattern });

        private static IReadOnlyDictionary<(string input, string pattern), ValidTestCase> InputToValidTestCase => ValidTestCases
            .ToDictionary(x => (x.Input, x.Pattern));

        private static IEnumerable<ValidTestCase> ValidTestCases => new[]
        {
            new ValidTestCase
            {
                Input = "foo bar baz",
                Pattern = "foo {} baz",
                Values = new[] { "bar" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "bar" } },
            },
            new ValidTestCase
            {
                Input = "abc foo bar baz xyz",
                Pattern = "foo {} baz",
                Values = new[] { "bar" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "bar" } },
            },
            new ValidTestCase
            {
                Input = "000 abc foo bar baz xyz 000",
                Pattern = "abc {} xyz",
                Values = new[] { "foo bar baz" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "foo bar baz" } },
            },
            new ValidTestCase
            {
                Input = "000 abc foo\nbar\nbaz xyz 000",
                Pattern = "abc {} xyz",
                Values = new[] { "foo\nbar\nbaz" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "foo\nbar\nbaz" } },
            },
            new ValidTestCase
            {
                Input = "abc",
                Pattern = "a{letter}c",
                Values = new[] { "b" },
                NameToValue = new Dictionary<string, string> { { "letter", "b" } },
            },
            new ValidTestCase
            {
                Input = "ab cd",
                Pattern = "a{1} {2}d",
                Values = new[] { "b", "c" },
                NameToValue = new Dictionary<string, string> { { "1", "b" }, { "2", "c" } },
            },
            new ValidTestCase
            {
                Input = "ab cd",
                Pattern = "a{foo} {foo}d",
                Values = new[] { "b", "c" },
                NameToValue = new Dictionary<string, string> { { "foo", "c" } },
            },
            new ValidTestCase
            {
                Input = "ab cd",
                Pattern = "a{ } {  }d",
                Values = new[] { "b", "c" },
                NameToValue = new Dictionary<string, string> { { " ", "b" }, { "  ", "c" } },
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "foo\n{}{}",
                Values = new[] { "bar\nba", "z" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "z" } },
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "{}{}\nbaz",
                Values = new[] { "foo\nba", "r" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "r" } },
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "{}{}\nbar",
                Values = new[] { "fo", "o" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "o" } },
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "{}{}{}\nbar",
                Values = new[] { "f", "o", "o" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "o" } },
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "{}{}{}",
                Values = new[] { "foo\nbar\nb", "a", "z" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "z" } },
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "\n{}{}{}",
                Values = new[] { "bar\nb", "a", "z" },
                NameToValue = new Dictionary<string, string> { { string.Empty, "z" } },
            },
            new ValidTestCase
            {
                Input = "   ",
                Pattern = "{}",
                Values = new[] { "   " },
                NameToValue = new Dictionary<string, string> { { string.Empty, "   " } },
            },
            new ValidTestCase
            {
                Input = "a b c",
                Pattern = " b ",
                Values = Array.Empty<string>(),
                NameToValue = new Dictionary<string, string>(),
            },
            new ValidTestCase
            {
                Input = "a b c",
                Pattern = "a b c",
                Values = Array.Empty<string>(),
                NameToValue = new Dictionary<string, string>(),
            },

            new ValidTestCase
            {
                Input = "foo bar baz",
                Pattern = "foo {} quz",
            },
            new ValidTestCase
            {
                Input = "",
                Pattern = "{}",
            },
            new ValidTestCase
            {
                Input = "foo\nbar\nbaz",
                Pattern = "{}{}{}{}\nbar",
            },
        };

        private record ValidTestCase
        {
            public string Input { get; init; }
            public string Pattern { get; init; }
            public IReadOnlyList<string> Values { get; init; }
            public IReadOnlyDictionary<string, string> NameToValue { get; init; }
        }
    }
}
