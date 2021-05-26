/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if NET48
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

namespace System.Runtime.CompilerServices
{
	public readonly struct ConfiguredAsyncDisposable
	{
		private readonly IAsyncDisposable _async_disposable;
		private readonly bool             _continue_on_captured_context;

		internal ConfiguredAsyncDisposable(IAsyncDisposable asyncDisposable, bool continueOnCapturedContext)
		{
			_async_disposable             = asyncDisposable;
			_continue_on_captured_context = continueOnCapturedContext;
		}

		public ConfiguredValueTaskAwaitable DisposeAsync()
		{
			return _async_disposable.DisposeAsync().ConfigureAwait(_continue_on_captured_context);
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
