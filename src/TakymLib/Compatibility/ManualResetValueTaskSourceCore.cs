/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if NET48
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks.Sources
{
	public sealed class ManualResetValueTaskSourceCore<TResult>
	{
		private TResult?               _result;
		private ExceptionDispatchInfo? _edi;
		private ValueTaskSourceStatus  _status;
		private Action<object?>?       _continuation;
		private object?                _continuation_state;
		public  short                  Version => 0;

		public void Reset()
		{
			lock (this) {
				_result = default;
				_status = ValueTaskSourceStatus.Pending;
			}
		}

		public TResult? GetResult(short token)
		{
			lock (this) {
				_edi?.Throw();
				return _result;
			}
		}

		public void SetResult(TResult? result)
		{
			lock (this) {
				_result = result;
				_status = ValueTaskSourceStatus.Succeeded;
				_continuation?.Invoke(_continuation_state);
			}
		}

		public void SetException(Exception? exception)
		{
			lock (this) {
				_status = ValueTaskSourceStatus.Faulted;
				_edi    = ExceptionDispatchInfo.Capture(exception);
				_continuation?.Invoke(_continuation_state);
			}
		}

		public ValueTaskSourceStatus GetStatus(short token)
		{
			lock (this) {
				return _status;
			}
		}

		public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
		{
			lock (this) {
				if (_continuation is null) {
					_continuation       = continuation;
					_continuation_state = state;
				} else {
					_continuation = state => {
						if (state is ContinuationState s) {
							s._1(s._1_state);
							s._2(s._2_state);
						}
					};
					_continuation_state = new ContinuationState(_continuation, continuation, _continuation_state, state);
				}
			}
		}

		private sealed class ContinuationState
		{
			internal readonly Action<object?> _1;
			internal readonly Action<object?> _2;
			internal readonly object?         _1_state;
			internal readonly object?         _2_state;

			internal ContinuationState(Action<object?> a, Action<object?> b, object? stateA, object? stateB)
			{
				_1       = a;
				_2       = b;
				_1_state = stateA;
				_2_state = stateB;
			}
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
