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
	public readonly struct AsyncIteratorMethodBuilder
	{
		public static AsyncIteratorMethodBuilder Create()
		{
			return new();
		}

		public void MoveNext<TStateMachine>(ref TStateMachine stateMachine)
			where TStateMachine: IAsyncStateMachine
		{
			stateMachine.MoveNext();
		}

		public void Complete() { }

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : INotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			awaiter.OnCompleted(new MoveNextAction(stateMachine).MoveNext);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : ICriticalNotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			awaiter.UnsafeOnCompleted(new MoveNextAction(stateMachine).MoveNext);
		}

		private sealed class MoveNextAction
		{
			private readonly IAsyncStateMachine _state_machine;

			internal MoveNextAction(IAsyncStateMachine stateMachine)
			{
				_state_machine = stateMachine;
			}

			internal void MoveNext()
			{
				_state_machine.MoveNext();
			}
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
