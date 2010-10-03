using System;
using System.Collections;
using System.Collections.Generic;

namespace Rasterizr.Util
{
	public class PartiallyActiveCollection<T> : IList<T>
	{
		private readonly List<T> _inner;
		private bool _locked;

		public int ActiveLength { get; set; }

		public PartiallyActiveCollection(int capacity)
		{
			_inner = new List<T>(capacity);
			ActiveLength = capacity;
		}

		public void Lock()
		{
			_locked = true;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < ActiveLength; ++i)
				yield return _inner[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			if (_locked)
				throw new InvalidOperationException();
			_inner.Add(item);
		}

		public void Clear()
		{
			if (_locked)
				throw new InvalidOperationException();
			_inner.Clear();
		}

		public bool Contains(T item)
		{
			return _inner.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_inner.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			if (_locked)
				throw new InvalidOperationException();
			return _inner.Remove(item);
		}

		public int Count
		{
			get { return ActiveLength; }
		}

		public bool IsReadOnly
		{
			get { return _locked; }
		}

		public int IndexOf(T item)
		{
			return _inner.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			if (_locked)
				throw new InvalidOperationException();
			_inner.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			if (_locked)
				throw new InvalidOperationException();
			_inner.RemoveAt(index);
		}

		public T this[int index]
		{
			get { return _inner[index]; }
			set { _inner[index] = value; }
		}
	}
}