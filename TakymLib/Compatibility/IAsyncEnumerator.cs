/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if NET48
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

using System.Threading.Tasks;

namespace System.Collections.Generic
{
	public interface IAsyncEnumerator<out T> : IAsyncDisposable
	{
		public T Current { get; }

		public ValueTask<bool> MoveNextAsync();
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
