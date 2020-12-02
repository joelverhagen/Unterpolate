using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Knapcode.Unterpolate.Samples
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Patterns look like string interpolation:");
            Console.WriteLine();
            ShowStringInterpolation();
            Console.WriteLine();

            Console.WriteLine("Result data can be used with string.Format:");
            Console.WriteLine();
            ShowStringFormat();
            Console.WriteLine();
        }

        private static void ShowStringInterpolation()
        {
            #region ShowStringInterpolation
            var first = "quick";
            var second = "lazy";

            var input = $"The {first} brown fox jumps over the {second} dog";
            var format = "The {first} brown fox jumps over the {second} dog";
            Console.WriteLine("  Input:    " + input);
            Console.WriteLine("  Format:   " + format);
            Console.WriteLine();

            var result = Extractor.Extract(input, format);
            Console.WriteLine("  Data:     " + JsonConvert.SerializeObject(result.NameToValue));
            #endregion
        }

        private static void ShowStringFormat()
        {
            #region ShowStringFormat
            var input = "The quick brown fox jumps over the lazy dog";
            var format = "The {0} brown fox jumps over the {1} dog";
            Console.WriteLine("  Input:    " + input);
            Console.WriteLine("  Format:   " + format);
            Console.WriteLine();

            var result = Extractor.Extract(input, format);
            var paramArray = result.ToArray();
            var reversed = result.Reverse().Cast<object>().ToArray();
            Console.WriteLine("  Data:     " + JsonConvert.SerializeObject(result.NameToValue));
            Console.WriteLine("  Same:     " + string.Format(CultureInfo.InvariantCulture, format, paramArray));
            Console.WriteLine("  Reversed: " + string.Format(CultureInfo.InvariantCulture, format, reversed));
            #endregion
        }
    }
}
