using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Knapcode.Unterpolate
{
    public partial class TokenizerTests
    {
        [Theory]
        [MemberData(nameof(InvalidTestData))]
        internal void RejectsInvalidInput(string input)
        {
            var testCase = InputToInvalidTestCase[input];

            var ex = Assert.Throws<FormatException>(() => Tokenizer.Tokenize(input).ToList());

            Assert.Equal(testCase.ExceptionMessage, ex.Message);
        }

        public static IEnumerable<object[]> InvalidTestData => InvalidTestCases
            .Select(x => new object[] { x.Input });

        private static IReadOnlyDictionary<string, InvalidTestCase> InputToInvalidTestCase => InvalidTestCases
            .ToDictionary(x => x.Input);

        private static IEnumerable<InvalidTestCase> InvalidTestCases => new[]
        {
            new InvalidTestCase
            {
                Input = "{",
                ExceptionMessage = "There is a capture section starting at index 0 without a terminating '}' character.",
            },
            new InvalidTestCase
            {
                Input = "foo { bar",
                ExceptionMessage = "There is a capture section starting at index 4 without a terminating '}' character.",
            },
            new InvalidTestCase
            {
                Input = "}",
                ExceptionMessage = "At index 0, there is an unescaped '}' character.",
            },
            new InvalidTestCase
            {
                Input = "}}}",
                ExceptionMessage = "At index 2, there is an unescaped '}' character.",
            },
            new InvalidTestCase
            {
                Input = "foo } bar",
                ExceptionMessage = "At index 4, there is an unescaped '}' character.",
            },
            new InvalidTestCase
            {
                Input = "{}{",
                ExceptionMessage = "There is a capture section starting at index 2 without a terminating '}' character.",
            },
            new InvalidTestCase
            {
                Input = "{{}",
                ExceptionMessage = "At index 2, there is an unescaped '}' character.",
            },
            new InvalidTestCase
            {
                Input = "foo { {bar} } baz",
                ExceptionMessage = "At index 6, there is an unexpected '{' character.",
            },
            new InvalidTestCase
            {
                Input = "foo { {{bar}} } baz",
                ExceptionMessage = "At index 6, there is an unexpected '{' character.",
            },
        };

        private record InvalidTestCase
        {
            public string Input { get; init; }
            public string ExceptionMessage { get; init; }
        }
    }
}
