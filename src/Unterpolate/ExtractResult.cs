using System;
using System.Collections;
using System.Collections.Generic;

namespace Knapcode.Unterpolate
{
    public class ExtractResult : IReadOnlyList<string>
    {
        private readonly IReadOnlyList<string> _values;
        private readonly IReadOnlyDictionary<string, string> _nameToValue;

        internal static ExtractResult Unsuccessful { get; } = new ExtractResult(
            success: false,
            Array.Empty<string>(),
            new Dictionary<string, string>());

        internal ExtractResult(bool success, IReadOnlyList<string> values, IReadOnlyDictionary<string, string> nameToValue)
        {
            Success = success;
            _values = values ?? throw new ArgumentNullException(nameof(values));
            _nameToValue = nameToValue ?? throw new ArgumentNullException(nameof(values));

            if (values.Count < nameToValue.Count)
            {
                throw new ArgumentException("The must not be more named values than total values.");
            }
        }

        public bool Success { get; }
        public IReadOnlyDictionary<string, string> NameToValue => _nameToValue;

        public string this[int index] => _values[index];
        public int Count => _values.Count;
        public IEnumerator<string> GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        public string this[string key] => _nameToValue[key];
        public bool ContainsKey(string key) => _nameToValue.ContainsKey(key);
        public bool TryGetValue(string key, out string value) => _nameToValue.TryGetValue(key, out value);
    }
}
