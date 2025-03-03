// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Collections.Generic
{
    // Provides the Create factory method for KeyValuePair<TKey, TValue>.
    public static class KeyValuePair
    {
        // Creates a new KeyValuePair<TKey, TValue> from the given values.
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
            new KeyValuePair<TKey, TValue>(key, value);

        /// <summary>Used by KeyValuePair.ToString to reduce generic code</summary>
        internal static string PairToString(object? key, object? value) => $"[{key}, {value}]";
            // string.Create(null, stackalloc char[256], $"[{key}, {value}]");
    }

    // A KeyValuePair holds a key and a value from a dictionary.
    // It is used by the IEnumerable<T> implementation for both IDictionary<TKey, TValue>
    // and IReadOnlyDictionary<TKey, TValue>.
    public readonly struct KeyValuePair<TKey, TValue>
    {
        private readonly TKey key; // Do not rename (binary serialization)
        private readonly TValue value; // Do not rename (binary serialization)

        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key => key;

        public TValue Value => value;

        public override string ToString()
        {
            return KeyValuePair.PairToString(Key, Value);
        }

        public void Deconstruct(out TKey key, out TValue value)
        {
            key = Key;
            value = Value;
        }
    }
}
