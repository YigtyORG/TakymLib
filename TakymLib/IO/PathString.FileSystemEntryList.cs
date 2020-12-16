/****
 * TakymLib
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections;
using System.Collections.Generic;

namespace TakymLib.IO
{
	partial class PathString
	{
		private readonly struct FileSystemEntryList : IEnumerable<PathString>, IEnumerator<PathString>
		{
			private readonly IEnumerator<string> _entries;

			public PathString Current => new PathString(_entries.Current);

			object? IEnumerator.Current => this.Current;

			internal FileSystemEntryList(IEnumerable<string> entries)
			{
				_entries = entries.GetEnumerator();
			}

			public bool MoveNext()
			{
				return _entries.MoveNext();
			}

			public void Reset()
			{
				_entries.Reset();
			}

			public void Dispose()
			{
				_entries.Dispose();
			}

			public IEnumerator<PathString> GetEnumerator()
			{
				return this;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
