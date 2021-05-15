/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TakymLib.Threading.Tasks;

namespace TakymLib.Aspect
{
	/// <summary>
	///  <see cref="TakymLib.Aspect.LoggableTask"/>を利用した非同期関数を生成します。
	/// </summary>
	public struct AsyncLoggableTaskMethodBuilder : IAsyncMethodBuilder<LoggableTask>
	{
		private AsyncValueTaskMethodBuilder _builder;
		private string?                     _member_name;
		private string?                     _file_path;
		private int                         _line_number;

		/// <summary>
		///  <see cref="TakymLib.Aspect.LoggableTask"/>を取得します。
		/// </summary>
		public LoggableTask Task => new(_builder.Task);

		/// <inheritdoc/>
		public void Start<TStateMachine>(ref TStateMachine stateMachine)
			where TStateMachine: IAsyncStateMachine
		{
			var frame = new StackFrame(1);
			var minfo = frame.GetMethod();
			_member_name = minfo?.Name;
			_file_path   = frame.GetFileName() ?? minfo?.DeclaringType?.AssemblyQualifiedName;
			_line_number = frame.GetFileLineNumber();

			LoggableTask.Logger?.Begin(_member_name ?? string.Empty, _file_path ?? string.Empty, _line_number);

			_builder.Start(ref stateMachine);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStateMachine(IAsyncStateMachine? stateMachine)
		{
			_builder.SetStateMachine(stateMachine!);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetException(Exception e)
		{
			_builder.SetException(e);
			LoggableTask.Logger?.End(e, _member_name ?? string.Empty, _file_path ?? string.Empty, _line_number);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetResult()
		{
			_builder.SetResult();
			LoggableTask.Logger?.End(_member_name ?? string.Empty, _file_path ?? string.Empty, _line_number);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : INotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			_builder.AwaitOnCompleted(ref awaiter, ref stateMachine);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : ICriticalNotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			_builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
		}

		/// <summary>
		///  <see cref="TakymLib.Aspect.AsyncLoggableTaskMethodBuilder"/>を作成します。
		/// </summary>
		/// <returns><see cref="TakymLib.Aspect.AsyncLoggableTaskMethodBuilder"/>を返します。</returns>
		public static AsyncLoggableTaskMethodBuilder Create()
		{
			var result = new AsyncLoggableTaskMethodBuilder();
			result._line_number = -1;
			return result;
		}
	}
}
