# Unterpolate

![.NET](https://github.com/joelverhagen/Unterpolate/workflows/.NET/badge.svg)

The opposite of string interpolation.

Parse a string into a sequence of values or name-value pairs using a format string.

The format string looks like [C# string interpolation](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated)
or the `string.Format` format string (a.k.a. [composite formatting](https://docs.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting)).

## Examples

### Parse using string interpolation formats

<!-- CODE-REGION samples\Unterpolate.Samples\Program.cs ShowStringInterpolation -->
```csharp
var first = "quick";
var second = "lazy";

var input = $"The {first} brown fox jumps over the {second} dog";
var format = "The {first} brown fox jumps over the {second} dog";
Console.WriteLine("  Input:    " + input);
Console.WriteLine("  Format:   " + format);
Console.WriteLine();

var result = Extractor.Extract(input, format);
Console.WriteLine("  Data:     " + JsonConvert.SerializeObject(result.NameToValue));
```
<!-- /CODE-REGION -->

Output:
<!-- COMMAND-OUTPUT dotnet run -p samples/Unterpolate.Samples ShowStringInterpolation -->
```
  Input:    The quick brown fox jumps over the lazy dog
  Format:   The {first} brown fox jumps over the {second} dog

  Data:     {"first":"quick","second":"lazy"}
```
<!-- /COMMAND-OUTPUT -->

### Parse using `string.Format` formats

<!-- CODE-REGION samples\Unterpolate.Samples\Program.cs ShowStringFormat -->
```csharp
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
```
<!-- /CODE-REGION -->

Output:
<!-- CODE-REGION-OUTPUT samples\Unterpolate.Samples\Program.cs ShowStringFormat -->
```
  Input:    The quick brown fox jumps over the lazy dog
  Format:   The {0} brown fox jumps over the {1} dog

  Data:     {"0":"quick","1":"lazy"}
  Same:     The quick brown fox jumps over the lazy dog
  Reversed: The lazy brown fox jumps over the quick dog
```
<!-- /CODE-REGION-OUTPUT -->