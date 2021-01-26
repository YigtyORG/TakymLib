﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Tasks.Wrappers
{
	/// <summary>
	///  <see cref="System.Threading.Tasks.ValueTask{TResult}"/>を<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult{TResult}"/>として扱える様にします。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct ValueTaskWrapper<TResult> : IAsyncMethodResult<TResult>
	{
		private readonly ValueTask<TResult> _task;

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public object? AsyncState => throw new NotSupportedException();

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public WaitHandle AsyncWaitHandle => throw new NotSupportedException();

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public Exception? Exception => throw new NotSupportedException();

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsCompleted"/>の値を取得します。
		/// </summary>
		public bool IsCompleted => _task.IsCompleted;

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsCompletedSuccessfully"/>の値を取得します。
		/// </summary>
		public bool IsCompletedSuccessfully => _task.IsCompletedSuccessfully;

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsFaulted"/>の値を取得します。
		/// </summary>
		public bool IsFailed => _task.IsFaulted;

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsCanceled"/>の値を取得します。
		/// </summary>
		public bool IsCancelled => _task.IsCanceled;

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public bool CompletedSynchronously => throw new NotSupportedException();

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.ValueTaskWrapper{TResult}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		public ValueTaskWrapper(ValueTask<TResult> task)
		{
			task.EnsureNotNull(nameof(task));
			_task = task;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.GetAwaiter"/>を呼び出します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.ValueTaskAwaiterWrapper{TResult}"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTaskAwaiterWrapper<TResult> GetAwaiter()
		{
			return new(_task.GetAwaiter());
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.ConfigureAwait(bool)"/>を呼び出します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元に実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </param>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredValueTaskAwaitableWrapper{TResult}"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredValueTaskAwaitableWrapper<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_task.ConfigureAwait(continueOnCapturedContext));
		}

		IAwaiter<TResult> IAwaitable<TResult>.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		IAwaitable<TResult> IAsyncMethodResult<TResult>.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
	}
}
