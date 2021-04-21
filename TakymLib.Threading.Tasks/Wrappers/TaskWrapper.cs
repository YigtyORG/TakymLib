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
	///  <see cref="System.Threading.Tasks.Task"/>を<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult"/>として扱える様にします。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct TaskWrapper : IAsyncMethodResult
	{
		private readonly Task _task;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.AsyncState"/>の値を取得します。
		/// </summary>
		public object? AsyncState
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.AsyncState;
		}

		/// <summary>
		///  現在の<see cref="System.Threading.Tasks.Task"/>から<see cref="System.IAsyncResult.AsyncState"/>の値を取得します。
		/// </summary>
		public WaitHandle AsyncWaitHandle
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ((IAsyncResult)(_task)).AsyncWaitHandle;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.Exception"/>の値を取得します。
		/// </summary>
		public Exception? Exception
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.Exception;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsCompleted"/>の値を取得します。
		/// </summary>
		public bool IsCompleted
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCompleted;
		}
		
#if NET48
		/// <inheritdoc/>
#else
		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsCompletedSuccessfully"/>の値を取得します。
		/// </summary>
#endif
		public bool IsCompletedSuccessfully
		{
#if NET48
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCompleted && !_task.IsFaulted;
#else
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCompletedSuccessfully;
#endif
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsFaulted"/>の値を取得します。
		/// </summary>
		public bool IsFailed
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsFaulted;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsCanceled"/>の値を取得します。
		/// </summary>
		public bool IsCancelled
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCanceled;
		}

		/// <summary>
		///  現在の<see cref="System.Threading.Tasks.Task"/>から<see cref="System.IAsyncResult.CompletedSynchronously"/>の値を取得します。
		/// </summary>
		public bool CompletedSynchronously
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ((IAsyncResult)(_task)).CompletedSynchronously;
		}

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.TaskWrapper"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskWrapper(Task task)
		{
			task.EnsureNotNull(nameof(task));
			_task = task;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.GetAwaiter"/>を呼び出します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.TaskAwaiterWrapper"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiterWrapper GetAwaiter()
		{
			return new(_task.GetAwaiter());
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.ConfigureAwait(bool)"/>を呼び出します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元に実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </param>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredTaskAwaitableWrapper"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredTaskAwaitableWrapper ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_task.ConfigureAwait(continueOnCapturedContext));
		}

		IAwaiter IAwaitable.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		IAwaitable IAsyncMethodResult.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
	}
}