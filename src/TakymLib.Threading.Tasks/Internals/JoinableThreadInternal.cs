/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Tasks.Internals
{
	internal sealed class JoinableThreadInternal : JoinableThread
	{
		private readonly ConcurrentQueue<Action> _queue;
		private readonly CancellationTokenSource _cts;

		public override Thread Thread { get; }

		internal JoinableThreadInternal()
		{
			_queue = new();
			_cts   = new();

			this.Thread = new(this.RunLoop);
			this.Thread.IsBackground = true;
			this.Thread.Start();
		}

		private void RunLoop()
		{
			while (!_cts.IsCancellationRequested) {
				if (_queue.TryDequeue(out var action)) {
					action();
				}
				Thread.Yield();
			}
		}

		protected override void ScheduleCore(Action action)
		{
			_queue.Enqueue(action);
		}

		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
#if !NET48
			_queue.Clear();
#endif
			if (disposing) {
				_cts.Cancel();
				_cts.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			_cts.Cancel();
			if (_cts is IAsyncDisposable asyncDisposable) {
				await asyncDisposable.ConfigureAwait(false).DisposeAsync();
			} else {
				_cts.Dispose();
			}
			await base.DisposeAsyncCore();
		}
	}
}
