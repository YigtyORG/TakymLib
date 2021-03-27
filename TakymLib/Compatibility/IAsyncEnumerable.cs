/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if NET48
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

using System.Threading;

namespace System.Collections.Generic
{
	public interface IAsyncEnumerable<out T>
	{
		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
