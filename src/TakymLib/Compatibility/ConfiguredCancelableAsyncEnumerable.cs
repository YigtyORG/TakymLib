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
using System.Threading;

namespace System.Runtime.CompilerServices
{
	public readonly struct ConfiguredCancelableAsyncEnumerable<T>
	{
		private readonly IAsyncEnumerable<T> _async_enumerable;
		private readonly bool                _continue_on_captured_context;
		private readonly CancellationToken   _cancellation_token;

		internal ConfiguredCancelableAsyncEnumerable(IAsyncEnumerable<T> asyncEnumerable, bool continueOnCapturedContext)
		{
			_async_enumerable             = asyncEnumerable;
			_continue_on_captured_context = continueOnCapturedContext;
			_cancellation_token           = default;
		}

		internal ConfiguredCancelableAsyncEnumerable(IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken)
		{
			_async_enumerable             = asyncEnumerable;
			_continue_on_captured_context = false;
			_cancellation_token           = cancellationToken;
		}

		private ConfiguredCancelableAsyncEnumerable(IAsyncEnumerable<T> asyncEnumerable, bool continueOnCapturedContext, CancellationToken cancellationToken)
		{
			_async_enumerable             = asyncEnumerable;
			_continue_on_captured_context = continueOnCapturedContext;
			_cancellation_token           = cancellationToken;
		}

		public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_async_enumerable, continueOnCapturedContext, _cancellation_token);
		}

		public ConfiguredCancelableAsyncEnumerable<T> WithCancellation(CancellationToken cancellationToken)
		{
			return new(_async_enumerable, _continue_on_captured_context, cancellationToken);
		}

		public Enumerator GetAsyncEnumerator()
		{
			return new(_async_enumerable.GetAsyncEnumerator(_cancellation_token), _continue_on_captured_context);
		}

		public readonly struct Enumerator
		{
			private readonly IAsyncEnumerator<T> _async_enumerator;
			private readonly bool                _continue_on_captured_context;

			public T Current => _async_enumerator.Current;

			internal Enumerator(IAsyncEnumerator<T> asyncEnumerator, bool continueOnCapturedContext)
			{
				_async_enumerator             = asyncEnumerator;
				_continue_on_captured_context = continueOnCapturedContext;
			}

			public ConfiguredValueTaskAwaitable<bool> MoveNextAsync()
			{
				return _async_enumerator.MoveNextAsync().ConfigureAwait(_continue_on_captured_context);
			}

			public ConfiguredValueTaskAwaitable DisposeAsync()
			{
				return _async_enumerator.DisposeAsync().ConfigureAwait(_continue_on_captured_context);
			}
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
