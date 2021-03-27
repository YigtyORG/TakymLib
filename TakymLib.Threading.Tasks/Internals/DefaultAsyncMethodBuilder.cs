/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;

namespace TakymLib.Threading.Tasks.Internals
{
	internal sealed class DefaultAsyncMethodBuilder<TResult> : IAsyncMethodBuilder<TResult>
	{
		private readonly DefaultAsyncMethodResult<TResult> _task;
		private          bool                              _will_complete_sync;

		public IAsyncMethodResult<TResult> Task => _task;

		internal DefaultAsyncMethodBuilder()
		{
			_task               = new DefaultAsyncMethodResult<TResult>();
			_will_complete_sync = true;
		}

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			stateMachine.EnsureNotNull(nameof(stateMachine));
			stateMachine.MoveNext();
		}

		public void SetStateMachine(IAsyncStateMachine? stateMachine)
		{
			_task.AsyncState = stateMachine;
		}

		public void SetException(Exception e)
		{
			_task.SetException(e, _will_complete_sync);
		}

		public void SetResult(TResult? result)
		{
			_task.SetResult(result, _will_complete_sync);
		}

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : INotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			awaiter     .EnsureNotNull(nameof(awaiter));
			stateMachine.EnsureNotNull(nameof(stateMachine));
			_will_complete_sync = false;
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : ICriticalNotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			awaiter     .EnsureNotNull(nameof(awaiter));
			stateMachine.EnsureNotNull(nameof(stateMachine));
			_will_complete_sync = false;
			awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
		}

#if !NETCOREAPP3_1_OR_GREATER
		IAsyncMethodResult          ICustomAsyncMethodBuilder<IAsyncMethodResult>         .Task => this.Task;
		IAsyncMethodResult<TResult> ICustomAsyncMethodBuilder<IAsyncMethodResult<TResult>>.Task => this.Task;

		void ICustomAsyncMethodBuilder<IAsyncMethodResult>.SetResult()
		{
			this.SetResult(default);
		}

		void ICustomAsyncMethodBuilder<IAsyncMethodResult<TResult>>.SetResult()
		{
			this.SetResult(default);
		}
#endif
	}
}
