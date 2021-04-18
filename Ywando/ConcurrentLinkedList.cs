/****
 * Ywando
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Ywando
{
	/// <summary>
	///  非同期的に操作する事ができる接続リストを表します。
	/// </summary>
	/// <typeparam name="T">リストで利用する型です。</typeparam>
	public class ConcurrentLinkedList<T> : IList<T>, IList, IReadOnlyList<T>
	{
		private volatile Entry? _first, _last;
		private          int    _count;
		private          ulong  _version;

		/// <summary>
		///  値を取得または設定します。
		/// </summary>
		/// <param name="index">値の位置です。</param>
		/// <returns><paramref name="index"/>にある値です。</returns>
		public T this[int index]
		{
			get => default;
			set { }
		}

		object? IList.this[int index]
		{
			get => default;
			set { }
		}

		/// <summary>
		///  このリスト内に格納されている要素の数を取得します。
		/// </summary>
		public int Count => _count;

		/// <summary>
		///  このリストが読み取り専用かどうかを示す論理値を取得します。
		/// </summary>
		/// <returns>
		///  このリストは書き込み可能である為、常に<see langword="false"/>を返します。
		/// </returns>
		public bool IsReadOnly => false;

		/// <summary>
		///  このリストの容量が固定されているかどうかを示す論理値を取得します。
		/// </summary>
		/// <returns>
		///  このリストは可変長である為、常に<see langword="false"/>を返します。
		/// </returns>
		public bool IsFixedSize => false;

		/// <summary>
		///  このリストへの操作が同期されるかどうかを示す論理値を取得します。
		/// </summary>
		/// <returns>
		///  このリストはスレッド安全である為、常に<see langword="true"/>を返します。
		/// </returns>
		public bool IsSynchronized => true;

		/// <summary>
		///  このリストへの操作の同期に利用できるオブジェクトを取得します。
		/// </summary>
		/// <returns>
		///  常に自身を返します。
		/// </returns>
		public object SyncRoot => this;

		/// <summary>
		///  型'<see cref="Ywando.ConcurrentLinkedList{T}"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ConcurrentLinkedList() { }

		/// <summary>
		///  型'<see cref="Ywando.ConcurrentLinkedList{T}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="array">新しいリストに格納する配列です。</param>
		public ConcurrentLinkedList(params T[] array)
		{
			this.AddRange(array);
		}

		/// <summary>
		///  型'<see cref="Ywando.ConcurrentLinkedList{T}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="enumerable">新しいリストに格納する<see cref="System.Collections.Generic.IEnumerable{T}"/>です。</param>
		public ConcurrentLinkedList(IEnumerable<T> enumerable)
		{
			this.AddRange(enumerable);
		}

		public void Add(T item)
		{
		}

		public int Add(object? value)
		{
			throw new NotImplementedException();
		}

		public void AddRange(params T[] array)
		{
		}

		public void AddRange(IEnumerable<T> enumerable)
		{
		}

		public void Insert(int index, T item)
		{
		}

		public void Insert(int index, object? value)
		{
		}

		public void InsertRange(params T[] array)
		{
		}

		public void InsertRange(IEnumerable<T> enumerable)
		{
		}

		private void InsertRangeCore(Entry head, Entry tail)
		{
			lock (this) {
				if (_first is null) {
					_first = head;
				}
				head._prev = _last;
				if (_last is not null) {
					_last._next = head;
				}
				_last = tail;
				++_count;
				++_version;
			}

			Interlocked.CompareExchange<Entry?>(ref _first, head, null);
			do {
				head._prev = _last;
				if (head._prev is not null) {
					
				}
			} while (Interlocked.CompareExchange(ref _last, tail, head._prev) != head._prev);

		}

		public bool Remove(T item)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(object? value)
		{
		}

		public void RemoveAt(int index)
		{
		}

		public void Clear()
		{
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
		}

		public void CopyTo(Array array, int index)
		{
		}

		public int IndexOf(T item)
		{
			throw new System.NotImplementedException();
		}

		public int IndexOf(object? value)
		{
			throw new NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(object? value)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private sealed class Entry
		{
			internal readonly T      _value;
			internal          Entry? _prev;
			internal          Entry? _next;

			internal Entry(T value)
			{
				_value = value;
			}
		}
	}
}
