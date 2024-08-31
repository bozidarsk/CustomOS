namespace System.Collections.Generic;

#pragma warning disable CS8600, CS8601, CS8604

public class Dictionary<TKey, TValue> : IDictionary, IDictionary<TKey, TValue>
{
	internal List<TKey> keys;
	internal List<TValue> values;

	public int Count => keys.Count;

	public object SyncRoot => this;
	public bool IsSynchronized => false;
	public bool IsReadOnly => false;
	public bool IsFixedSize => false;

	ICollection IDictionary.Keys => keys;
	public ICollection<TKey> Keys => keys;

	ICollection IDictionary.Values => values;
	public ICollection<TValue> Values => values;

	IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new Enumerator(this);

	public TValue this[TKey key] 
	{
		set 
		{
			int index = keys.IndexOf(key ?? throw new ArgumentNullException());
			values[index] = (index != -1) ? value : throw new KeyNotFoundException();
		}
		get 
		{
			int index = keys.IndexOf(key ?? throw new ArgumentNullException());
			return (index != -1) ? values[index] : throw new KeyNotFoundException();
		}
	}

	object? IDictionary.this[object key] 
	{
		set 
		{
			if (!(key is TKey) || !(value is TValue))
				throw new ArgumentException();

			int index = keys.IndexOf((TKey)key ?? throw new ArgumentNullException());
			values[index] = (index != -1) ? (TValue)value : throw new KeyNotFoundException();
		}
		get 
		{
			if (!(key is TKey))
				throw new ArgumentException();

			int index = keys.IndexOf((TKey)key ?? throw new ArgumentNullException());
			return (index != -1) ? values[index] : throw new KeyNotFoundException();
		}
	}

	bool IDictionary.Contains(object key) => (key is TKey || key == null) ? InternalContainsKey((TKey)key ?? throw new ArgumentNullException()) : throw new ArgumentException();
	void IDictionary.Add(object key, object? value) => InternalAdd((key is TKey || key == null) ? ((TKey)key ?? throw new ArgumentNullException()) : throw new ArgumentException(), (value is TValue || value == null) ? (TValue)value : throw new ArgumentException());
	void IDictionary.Remove(object key) => InternalRemove((key is TKey || key == null) ? ((TKey)key ?? throw new ArgumentNullException()) : throw new ArgumentException());
	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => InternalAdd(pair.Key, pair.Value);
	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair) => InternalContainsKey(pair.Key) && InternalContainsValue(pair.Value);
	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair) => InternalRemove(pair.Key);

	void ICollection.CopyTo(Array destination, int index) 
	{
		if (destination == null)
			throw new ArgumentNullException();
		if (index < 0)
			throw new ArgumentOutOfRangeException();
		if (destination.Rank != 1 || destination.Length != this.Count || !(destination is KeyValuePair<TKey, TValue>[]))
			throw new ArgumentException();

		for (int i = 0; i < this.Count; i++)
			((KeyValuePair<TKey, TValue>[])destination)[index + i] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] destination, int index) 
	{
		if (destination == null)
			throw new ArgumentNullException();
		if (index < 0)
			throw new ArgumentOutOfRangeException();
		if (destination.Rank != 1 || destination.Length != this.Count)
			throw new ArgumentException();

		for (int i = 0; i < this.Count; i++)
			destination[index + i] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
	}

	public bool ContainsKey(TKey key) => InternalContainsKey(key);
	public bool ContainsValue(TValue value) => InternalContainsValue(value);
	public void Add(TKey key, TValue value) => InternalAdd(key, value);
	public bool TryAdd(TKey key, TValue value) => InternalTryAdd(key, value);
	public bool TryGetValue(TKey key, out TValue value) => InternalTryGetValue(key, out value);
	public bool Remove(TKey key) => InternalRemove(key);
	public bool Remove(TKey key, out TValue value) => InternalRemove(key, out value);
	public void Clear() => InternalClear();

	private bool InternalContainsKey(TKey key) => (key != null) ? keys.Contains(key) : throw new ArgumentNullException();
	private bool InternalContainsValue(TValue value) => (value != null) ? values.Contains(value) : throw new ArgumentNullException();

	private void InternalAdd(TKey key, TValue value) 
	{
		if (key == null)
			throw new ArgumentNullException();

		if (!TryAdd(key, value))
			throw new ArgumentException();
	}

	private bool InternalTryAdd(TKey key, TValue value) 
	{
		if (key == null)
			throw new ArgumentNullException();

		if (keys.Contains(key))
			return false;

		keys.Add(key);
		values.Add(value);

		return true;
	}

	private bool InternalTryGetValue(TKey key, out TValue value) 
	{
		if (key == null)
			throw new ArgumentNullException();

		int index = keys.IndexOf(key);
		value = (index != -1) ? values[index] : default;

		return index != -1;
	}

	private bool InternalRemove(TKey key) 
	{
		if (key == null)
			throw new ArgumentNullException();

		int index = keys.IndexOf(key);

		if (index != -1) 
		{
			keys.RemoveAt(index);
			values.RemoveAt(index);
		}

		return index != -1;
	}

	private bool InternalRemove(TKey key, out TValue value) 
	{
		if (key == null)
			throw new ArgumentNullException();

		int index = keys.IndexOf(key);
		value = (index != -1) ? values[index] : default;

		if (index != -1) 
		{
			keys.RemoveAt(index);
			values.RemoveAt(index);
		}

		return index != -1;
	}

	private void InternalClear() 
	{
		keys.Clear();
		values.Clear();
	}

	public Dictionary(int capacity = 4) 
	{
		this.keys = new(capacity);
		this.values = new(capacity);
	}

	public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this()
	{
		foreach (var x in collection)
			this.Add(x.Key, x.Value);
	}

	public sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable
	{
		private Dictionary<TKey, TValue> dictionary;
		private int index = -1;

		object? IEnumerator.Current => Current;

		public KeyValuePair<TKey, TValue> Current => ((uint)index >= (uint)dictionary.Count) ? throw new InvalidOperationException() : new KeyValuePair<TKey, TValue>(dictionary.keys[index], dictionary.values[index]);

		public void Dispose() => dictionary = null!;

		public void Reset() => index = -1;

		public bool MoveNext() 
		{
			bool success = index + 1 < dictionary.Count;
			index = success ? index + 1 : dictionary.Count;
			return success;
		}

		internal Enumerator(Dictionary<TKey, TValue> dictionary) => this.dictionary = dictionary;
	}
}
