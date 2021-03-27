/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace TakymLib.Threading.Tasks.Internals
{
	internal sealed class DefaultAsyncMethodResult<TResult> : IAsyncMethodResult<TResult>, IAwaiter<TResult>
	{
		private TResult?   _result;
		private Action     _continuation;
		public  object?    AsyncState             { get; internal set; }
		public  WaitHandle AsyncWaitHandle        { get; }
		public  Exception? Exception              { get; private set; }
		public  bool       CompletedSynchronously { get; private set; }
		public  bool       IsCompleted            { get; private set; }

		internal DefaultAsyncMethodResult()
		{
			_continuation        = () => { };
			this.AsyncWaitHandle = EmptyWaitHandle.Instance;
		}

		public IAwaiter<TResult> GetAwaiter()
		{
			return this;
		}

		public TResult? GetResult()
		{
			bool wait;
			lock (this) {
				wait = !this.IsCompleted;
			}
			while (wait) {
				lock (this) {
					wait = !this.IsCompleted;
				}
				Thread.Yield();
			}
			if (this.Exception is not null) {
#if NET48
				ExceptionDispatchInfo.Capture(this.Exception).Throw();
#else
				ExceptionDispatchInfo.Throw(this.Exception);
#endif
			}
			return _result;
		}

		internal void SetException(Exception e, bool completedSynchronously)
		{
			e.EnsureNotNull(nameof(e));
			lock (this) {
				this.Exception              = e;
				this.CompletedSynchronously = completedSynchronously;
				this.IsCompleted            = true;
			}
		}

		internal void SetResult(TResult? result, bool completedSynchronously)
		{
			Action c;
			lock (this) {
				_result                     = result;
				this.CompletedSynchronously = completedSynchronously;
				this.IsCompleted            = true;
				c = _continuation;
			}
			c();
		}

		public void OnCompleted(Action continuation)
		{
			lock (this) {
				_continuation += continuation;
			}
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			this.OnCompleted(continuation);
		}

		public IAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new DefaultAsyncMethodResult<TResult>();
		}

		private sealed class EmptyWaitHandle : WaitHandle
		{
			internal static readonly EmptyWaitHandle Instance = new();

			private EmptyWaitHandle() { }
		}

#if !NETCOREAPP3_1_OR_GREATER
		public bool IsCompletedSuccessfully => this.IsCompleted && this.Exception is null;
		public bool IsFailed                => this.IsCompleted && this.Exception is not null;
		public bool IsCancelled             => this.IsCompleted && this.Exception is OperationCanceledException;

		IAwaiter IAwaitable.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		void IAwaiter.GetResult()
		{
			this.GetResult();
		}

		IAwaitable IAsyncMethodResult.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
#endif
	}
}
