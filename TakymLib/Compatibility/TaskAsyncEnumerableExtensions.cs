/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if NET48
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	public static class TaskAsyncEnumerableExtensions
	{
		public static ConfiguredAsyncDisposable ConfigureAwait(this IAsyncDisposable asyncDisposable, bool continueOnCapturedContext)
		{
			return new(asyncDisposable, continueOnCapturedContext);
		}

		public static ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait<T>(this IAsyncEnumerable<T> asyncEnumerable, bool continueOnCapturedContext)
		{
			return new(asyncEnumerable, continueOnCapturedContext);
		}

		public static ConfiguredCancelableAsyncEnumerable<T> WithCancellation<T>(this IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken)
		{
			return new(asyncEnumerable, cancellationToken);
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
