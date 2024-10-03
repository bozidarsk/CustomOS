using System;
using System.Collections;

namespace System.Collections.Generic;

#pragma warning disable CS8600, CS8601, CS8604

public class List<T> : IList, IList<T>, IReadOnlyList<T>
{
	private T[] array;
	private int next;

	public int Count => next;
	public int Capacity => array.Length;

	object ICollection.SyncRoot => this;
	bool ICollection.IsSynchronized => false;
	bool ICollection<T>.IsReadOnly => false;
	bool IList.IsReadOnly => false;
	bool IList.IsFixedSize => false;

	object? IList.this[int index] 
	{
		set => array[index] = (index < 0 || index >= Count) ? throw new ArgumentOutOfRangeException() : ((value is T || value == null) ? (T)value : throw new ArgumentException());
		get => (index < 0 || index >= Count) ? throw new ArgumentOutOfRangeException() : array[index];
	}

	public T this[int index] 
	{
		set => array[index] = (index < 0 || index >= Count) ? throw new ArgumentOutOfRangeException() : value;
		get => (index < 0 || index >= Count) ? throw new ArgumentOutOfRangeException() : array[index];
	}

	int IList.Add(object? item) 
	{
		if (item is T || item == null) 
		{
			InternalAdd((T)item);
			return next - 1;
		}

		throw new ArgumentException();
	}

	public void Add(T item) => InternalAdd(item);

	public void AddRange(IEnumerable<T> data) 
	{
		if (data == null)
			throw new ArgumentNullException();

		foreach (T item in data)
			Add(item);
	}

	void IList.Remove(object? item) 
	{
		if (!(item is T || item == null))
			throw new ArgumentException();

		int index = InternalIndexOf((T)item);

		if (index == -1)
			return;

		InternalRemoveAt(index);
	}

	public bool Remove(T item) 
	{
		int index = InternalIndexOf(item);

		if (index == -1)
			return false;

		InternalRemoveAt(index);
		return true;
	}

	void IList.RemoveAt(int index) => InternalRemoveAt((index < 0 || index >= this.Count) ? throw new ArgumentOutOfRangeException() : index);
	public void RemoveAt(int index) => InternalRemoveAt((index < 0 || index >= this.Count) ? throw new ArgumentOutOfRangeException() : index);

	void IList.Insert(int index, object? item) => InternalInsert((index < 0 || index >= this.Count) ? throw new ArgumentOutOfRangeException() : index, (item is T || item == null) ? (T)item : throw new ArgumentException());
	public void Insert(int index, T item) => InternalInsert((index < 0 || index >= this.Count) ? throw new ArgumentOutOfRangeException() : index, item);

	int IList.IndexOf(object? item) => (item is T || item == null) ? InternalIndexOf((T)item) : throw new ArgumentException();
	public int IndexOf(T item) => InternalIndexOf(item);

	bool IList.Contains(object? item) => (item is T || item == null) ? InternalIndexOf((T)item) >= 0 : throw new ArgumentException();
	public bool Contains(T item) => InternalIndexOf(item) >= 0;

	void IList.Clear() => InternalClear();
	public void Clear() => InternalClear();

	void ICollection.CopyTo(Array destination, int index) 
	{
		if (destination == null)
			throw new ArgumentNullException();
		if (index < 0)
			throw new ArgumentOutOfRangeException();
		if (destination.Rank != 1 || destination.Length != this.Count || !(destination is T[]))
			throw new ArgumentException();

		for (int i = 0; i < this.Count; i++)
			((T[])destination)[index + i] = array[i];
	}

	void ICollection<T>.CopyTo(T[] destination, int index) 
	{
		if (destination == null)
			throw new ArgumentNullException();
		if (index < 0)
			throw new ArgumentOutOfRangeException();
		if (destination.Rank != 1 || destination.Length != this.Count)
			throw new ArgumentException();

		for (int i = 0; i < this.Count; i++)
			destination[index + i] = array[i];
	}

	public T[] ToArray() 
	{
		T[] newArray = new T[this.Count];
		for (int i = 0; i < this.Count; i++)
			newArray[i] = array[i];

		return newArray;
	}

	IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);
	public IEnumerator<T> GetEnumerator() => new Enumerator(this);

	private void DoubleCapacity() 
	{
		T[] newArray = new T[array.Length * 2];

		for (int i = 0; i < array.Length; i++)
			newArray[i] = array[i];

		array = newArray;
	}

	private void InternalAdd(T item) 
	{
		if (next >= array.Length)
			DoubleCapacity();

		array[next++] = item;
	}

	private void InternalInsert(int index, T item) 
	{
		if (next >= array.Length)
			DoubleCapacity();

		for (int i = this.Count - 1; i >= index; i--)
			array[i + 1] = array[i];

		array[index] = item;
	}

	private void InternalRemoveAt(int index) 
	{
		for (int i = index; i < this.Count - 1; i++)
			array[i] = array[i + 1];
	}

	private int InternalIndexOf(T item) 
	{
		for (int i = 0; i < this.Count; i++) 
		{
			bool equals = false;

			if (item == null && array[i] != null)
				equals = array[i]!.Equals(item);

			if (item != null && array[i] == null)
				equals = item!.Equals(array[i]);

			if ((item == null && array[i] == null) || equals)
				return i;
		}

		return -1;
	}

	private void InternalClear() => next = 0;

	public List(int capacity = 4) 
	{
		if (capacity <= 0)
			throw new ArgumentOutOfRangeException();

		array = new T[capacity];
	}

	public List(IEnumerable<T> data) : this()
	{
		if (data == null)
			throw new ArgumentNullException();

		AddRange(data);
	}

	public sealed class Enumerator : IEnumerator<T>, IDisposable
	{
		private List<T> list;
		private int index = -1;

		object? IEnumerator.Current => Current;

		public T Current => ((uint)index >= (uint)list.Count) ? throw new InvalidOperationException() : list[index];

		public void Dispose() => list = null!;

		public void Reset() => index = -1;

		public bool MoveNext() 
		{
			bool success = index + 1 < list.Count;
			index = success ? index + 1 : list.Count;
			return success;
		}

		internal Enumerator(List<T> list) => this.list = list;
	}
}
